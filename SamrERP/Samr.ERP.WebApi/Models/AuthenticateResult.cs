using System;

namespace Samr.ERP.WebApi.Models
{
    public class AuthenticateResult
    {
        public Boolean IsSuccess { get; set; }
        public String Token { get; set; }
        public string Password { get; set; }
        public static AuthenticateResult Success(string token) => new AuthenticateResult(){IsSuccess = true, Token = token};
        public static AuthenticateResult Fail() => new AuthenticateResult() { IsSuccess = false};
        public static AuthenticateResult SuccessWithPassword(string password) => new AuthenticateResult(){IsSuccess = true, Password = password};

    }
}
