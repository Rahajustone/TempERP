using System;

namespace Samr.ERP.WebApi.Models
{
    public class AuthenticateResult
    {
        public String Token { get; set; }

        public AuthenticateResult()
        {
            
        }
        public AuthenticateResult(string token)
        {
            Token = token;
        }
    }
}
