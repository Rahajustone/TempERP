using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.WebApi.Configurations.Models;
using Samr.ERP.WebApi.ViewModels.Account;
using AuthenticateResult = Samr.ERP.WebApi.Models.AuthenticateResult;

namespace Samr.ERP.WebApi.Infrastructure
{
    public interface IAuthenticateService
    {
        Task<AuthenticateResult> IsAuthenticated(LoginViewModel loginModel);
    }
    public class TokenAuthenticationService:IAuthenticateService
    {
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;
        private readonly IOptions<AppSettings> _tokenSettings;


        public TokenAuthenticationService(
            IUserService userService,
            SignInManager<User> signInManager,
            IOptions<AppSettings> tokenSettings)
        {
            _userService = userService;
            _signInManager = signInManager;
            _tokenSettings = tokenSettings;
        }
        public async Task<AuthenticateResult> IsAuthenticated(LoginViewModel loginModel)
        {
            User user = await _userService.GetByUserName(loginModel.UserName);
            if (user == null) return AuthenticateResult.Fail();

            var checkPasswordResult = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
            if (!checkPasswordResult.Succeeded) return AuthenticateResult.Fail();

            var canSighInResult = await _signInManager.CanSignInAsync(user);
            if (!canSighInResult) return AuthenticateResult.Fail();

            var token = GetJwtTokenForUser(user);

            return AuthenticateResult.Success(token);
        }

        private string GetJwtTokenForUser(User user)
        {
            //TODO:Amir need to finish claims
            var claim = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Value.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //IdentityModelEventSource.ShowPII = true;
            var jwtToken = new JwtSecurityToken(
                _tokenSettings.Value.Issuer,
                _tokenSettings.Value.Audience,
                claim,
                expires: DateTime.Now.AddMinutes(_tokenSettings.Value.AccessExpiration),
                signingCredentials: credentials
            );
            
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
