using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.Infrastructure.Providers;

namespace Samr.ERP.WebApi.Middleware
{
    public class UserMiddleware
    {
        private readonly RequestDelegate next;

        public UserMiddleware(RequestDelegate next)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(
            HttpContext context,
            UserProvider userProvider,
            IUserService userService
        )
        {
            var user = await userService.GetUserAsync(context.User);

            if (user != null)
            {
                userProvider.Initialise(user);
            }
            await next(context);


           
        }
    }
}
