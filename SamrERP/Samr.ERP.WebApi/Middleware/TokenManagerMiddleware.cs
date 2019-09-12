using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Samr.ERP.Core.Interfaces;

namespace Samr.ERP.WebApi.Middleware
{
    public class TokenManagerMiddleware 
    {
        private readonly RequestDelegate _next;

        public TokenManagerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
       
        public async Task Invoke(
            HttpContext context, 
            IActiveUserTokenService activeUserTokenService,
            IAuthorizationService authorizationService)
        {
            var authorizationHeader = context.Request.Headers["authorization"];
            var accesstokenParam = context.Request.Query["access_token"];


            var tokenFromHeader = GetTokenValue(authorizationHeader);
            var tokenFromParam = GetTokenValue(accesstokenParam);

            if (!string.IsNullOrEmpty(tokenFromParam) && string.IsNullOrEmpty(tokenFromHeader))
                context.Request.Headers.Add("Authorization", $"Bearer {tokenFromParam}");

            var token = !string.IsNullOrEmpty(tokenFromHeader) ? tokenFromHeader : tokenFromParam;
            
            //AuthorizationResult authorizationResult =
            //    await authorizationService.AuthorizeAsync(context.User, null,"");

            if (string.IsNullOrEmpty(token) || await activeUserTokenService.TokenActive(token))
            {
                await _next(context);

                return;
            }
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }

        private static string GetTokenValue(StringValues authorizationHeader)
        {
            var token = authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();
            return token;
        }
    }
}
