using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
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
    public class TokenAuthenticationService:IAuthenticateService
    {
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;
        private readonly IOptions<AppSettings> _tokenSettings;
        private readonly IEmployeeService _employeeService;


        public TokenAuthenticationService(
            IUserService userService,
            SignInManager<User> signInManager,
            IOptions<AppSettings> tokenSettings,
            IEmployeeService employeeService
            )
        {
            _userService = userService;
            _signInManager = signInManager;
            _tokenSettings = tokenSettings;
            _employeeService = employeeService;
        }
        public async Task<BaseDataResponse<AuthenticateResult>> AuthenticateAsync(LoginViewModel loginModel)
        {
            var authenticateResult = new AuthenticateResult();
            User user = await _userService.GetByPhoneNumber(loginModel.PhoneNumber);
            if (user == null) return BaseDataResponse<AuthenticateResult>.Fail(null, new ErrorModel("login or pass not correct"));

            var checkPasswordResult = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
            if (!checkPasswordResult.Succeeded) return BaseDataResponse<AuthenticateResult>.Fail(null, new ErrorModel("login or pass not correct"));

            var canSighInResult = await _signInManager.CanSignInAsync(user);
            if (!canSighInResult) return BaseDataResponse<AuthenticateResult>.Fail(null,new ErrorModel("you cant login call to support"));

            var token = GetJwtTokenForUser(user);

            return BaseDataResponse<AuthenticateResult>.Success(new AuthenticateResult(token));
        }

        public Task<BaseDataResponse<AuthenticateResult>> RefreshTokenAsync(ExchangeRefreshToken model)
        {
            throw new NotImplementedException();
        }

        private string GetJwtTokenForUser(User user)
        {
            var employee = _employeeService.GetEmployeeInfo(user.Id);

            //TODO:Amir need to finish claims
            var claim = new[]
            {
                new Claim(ClaimTypes.Name, user.PhoneNumber),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("userId", user.Id.ToString()),
                new Claim("name", employee.Result.FullName),
                new Claim("position", employee.Result.PositionName),
                new Claim("photo", employee.Result.Photo),
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
