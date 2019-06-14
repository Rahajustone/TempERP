using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Samr.ERP.WebApi.Infrastructure;

namespace Samr.ERP.WebApi.Validators
{
    public class MyCustomUserValidator : UserValidator<ApplicationUser>
    {

        

        public MyCustomUserValidator(ApplicationUserManager appUserManager)
            : base(appUserManager)
        {
        }

        public override async Task<IdentityResult> ValidateAsync(ApplicationUser user)
        {
            IdentityResult result = await base.ValidateAsync(user);
          

            return result;
        }
    }

}