using System;

namespace Samr.ERP.WebApi.Models
{
    public class AuthenticateResult
    {
        public String Token { get; set; }
        public string RefreshToken { get; set; }
        public AuthenticateResult()
        {
            
        }
        public AuthenticateResult(string token,string refreshToken)
        {
            Token = token;
            RefreshToken = refreshToken;
        }
    }
}
