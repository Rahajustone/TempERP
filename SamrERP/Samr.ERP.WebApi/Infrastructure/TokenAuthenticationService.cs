using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Services;
using Samr.ERP.Core.ViewModels.Account;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.WebApi.Configurations.Models;
using AuthenticateResult = Samr.ERP.WebApi.Models.AuthenticateResult;

namespace Samr.ERP.WebApi.Infrastructure
{
    public interface IAuthenticateService
    {
        Task<BaseDataResponse<AuthenticateResult>> AuthenticateAsync(LoginViewModel loginModel);
        //Task<AuthenticateResult> ResetPasswordAsync(ResetPasswordViewModel resetPasswordModel);
        Task<BaseDataResponse<AuthenticateResult>> RefreshTokenAsync(ExchangeRefreshToken model);
    }
    public class TokenAuthenticationService : IAuthenticateService
    {
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;
        private readonly IOptions<AppSettings> _tokenSettings;
        private readonly IEmployeeService _employeeService;
        private readonly UserManager<User> _userManager;
        private readonly IActiveUserTokenService _activeUserTokenService;
        private readonly IHttpContextAccessor _accessor;
        private readonly SigningCredentials _signingCredentials;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;


        public TokenAuthenticationService(
            IUserService userService,
            SignInManager<User> signInManager,
            IOptions<AppSettings> tokenSettings,
            IEmployeeService employeeService,
            UserManager<User> userManager,
            IActiveUserTokenService activeUserTokenService,
            IHttpContextAccessor accessor
            )
        {
            _userService = userService;
            _signInManager = signInManager;
            _tokenSettings = tokenSettings;
            _employeeService = employeeService;
            _userManager = userManager;
            _activeUserTokenService = activeUserTokenService;
            _accessor = accessor;

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Value.Secret));
            _signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }
        public async Task<BaseDataResponse<AuthenticateResult>> AuthenticateAsync(LoginViewModel loginModel)
        {
            var authenticateResult = new AuthenticateResult();
            User user = await _userService.GetByPhoneNumber(loginModel.PhoneNumber);
            if (user == null) return BaseDataResponse<AuthenticateResult>.Fail(null, new ErrorModel("login or pass not correct"));

            var checkPasswordResult = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
            if (!checkPasswordResult.Succeeded) return BaseDataResponse<AuthenticateResult>.Fail(null, new ErrorModel("login or pass not correct"));

            var canSighInResult = await _signInManager.CanSignInAsync(user);
            if (!canSighInResult || user.LockDate != null) return BaseDataResponse<AuthenticateResult>.Fail(null,  new ErrorModel("you cant login call to support"));

            var token = GetJwtTokenForUser(user, loginModel.RememberMe);

            var refreshToken = GenerateTokenByRandomNumber();
            var remoteIpAddress = _accessor.HttpContext.Connection.RemoteIpAddress?.ToString();
            await _userService.ClearUserRefreshToken(user.Id);
            await _userService.AddRefreshToken(refreshToken, user.Id, remoteIpAddress); // add the new one
            await _activeUserTokenService.AddOrRefreshUserToken(user.Id, token);

            return BaseDataResponse<AuthenticateResult>.Success(new AuthenticateResult(token, refreshToken));
        }

        public async Task<BaseDataResponse<AuthenticateResult>> RefreshTokenAsync(ExchangeRefreshToken model)
        {
            var userPrincipal = GetPrincipalFromToken(model.AccessToken);
            var tokenActive = await _activeUserTokenService.TokenActive(model.AccessToken);
            // invalid token/signing key was passed and we can't extract user claims
            if (userPrincipal != null && tokenActive)
            {
                var userId = Guid.Parse(userPrincipal.Claims.First(c => c.Type == "id").Value);
                var ipAddress = _accessor.HttpContext.Connection.RemoteIpAddress?.ToString();
                if (_userService.HasUserValidRefreshToken(userId, model.RefreshToken, ipAddress))
                {
                    var user = await _userService.GetUserAsync(userPrincipal);
                    var jwtToken = GetJwtTokenForUser(user);

                    await _userService.ClearUserRefreshToken(user.Id);
                    //await _userService.RemoveRefreshToken(userId, model.RefreshToken); // delete the token we've exchanged

                    //TODO Refactor
                    var refreshToken = GenerateTokenByRandomNumber();
                    await _userService.AddRefreshToken(refreshToken, userId, _accessor.HttpContext.Connection.RemoteIpAddress?.ToString()); // add the new one
                    await _activeUserTokenService.AddOrRefreshUserToken(user.Id, jwtToken);

                    return BaseDataResponse<AuthenticateResult>.Success(new AuthenticateResult(jwtToken, refreshToken));

                }
            }
            return BaseDataResponse<AuthenticateResult>.Fail(null, new ErrorModel("invalid token"));
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenValidationParameters = GetTokenValidationParameters(_tokenSettings.Value);
            tokenValidationParameters.ValidateLifetime = false;
            var principal = _jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        private string GetJwtTokenForUser(User user, bool remember = false)
        {
            var employee = _employeeService.GetEmployeeInfo(user.Id);

            //TODO:Amir need to finish claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.PhoneNumber),
                new Claim("id", user.Id.ToString()),
                new Claim("name", employee.Result.FullName),
                new Claim("position", employee.Result.PositionName),
                new Claim("photo",employee.Result.Photo),
            };

            var roles = _userManager.GetRolesAsync(user).Result;

            claims.AddRange(roles.Select(p => new Claim(ClaimTypes.Role, p)));

            //IdentityModelEventSource.ShowPII = true;
            var accessExpiration = remember ? _tokenSettings.Value.RememberMeExpiration : _tokenSettings.Value.AccessExpiration;

            var jwtToken = new JwtSecurityToken(
                _tokenSettings.Value.Issuer,
                _tokenSettings.Value.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(accessExpiration),
                signingCredentials: _signingCredentials
            );

            return _jwtSecurityTokenHandler.WriteToken(jwtToken);
        }

        public static TokenValidationParameters GetTokenValidationParameters(AppSettings appSettings)
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.Secret)),
                ValidIssuer = appSettings.Issuer,
                ValidAudience = appSettings.Audience,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,


            };
        }

        public static string GenerateTokenByRandomNumber(int size = 32)
        {
            var randomNumber = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
