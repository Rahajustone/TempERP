using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Samr.ERP.WebApi.Models
{
    public class AuthenticateResult
    {
        public Boolean IsSuccess { get; set; }
        public String Token { get; set; }
        public string Password { get; set; }
        public static AuthenticateResult Success(string token) => new AuthenticateResult(){IsSuccess = true, Token = token};
        // TODO continue by Amir
        public static AuthenticateResult SuccessWithPassword(string password) => new AuthenticateResult(){IsSuccess = true, Password = password};
        public static AuthenticateResult Fail() => new AuthenticateResult() { IsSuccess = true};

    }
}
