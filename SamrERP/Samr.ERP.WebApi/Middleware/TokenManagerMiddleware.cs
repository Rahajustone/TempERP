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

            var token = authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();
            
            //AuthorizationResult authorizationResult =
            //    await authorizationService.AuthorizeAsync(context.User, null,"");

            if (
                (!String.IsNullOrEmpty(token) && await activeUserTokenService.TokenActive(token))
                || (!authorizationResult.Succeeded))
            {
                await _next(context);

                return;
            }
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}
