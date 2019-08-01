using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.Infrastructure.Extensions;

namespace Samr.ERP.Infrastructure.SeedData
{
    public static class DbInitializer
    {
        public static void AddRolesToSystemUser(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            var user = userManager.FindByIdAsync(GuidExtensions.FULL_GUID.ToString()).Result;
            if (user != null)
            {
                var roles = roleManager.Roles.Where(p => p.Category.ToUpper().Equals("EMPLOYEE"));
                foreach (var role in roles)
                {
                    if (!userManager.IsInRoleAsync(user, role.Name).Result)
                    {
                        var identityResult = userManager.AddToRoleAsync(user, role.Name).Result;
                    }
                }
            }
        }
    }
}
