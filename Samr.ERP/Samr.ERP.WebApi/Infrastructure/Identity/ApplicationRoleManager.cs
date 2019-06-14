using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Samr.ERP.WebApi.Infrastructure
{
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        private static ApplicationRoleManager _roleManager;
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore)
        {

        }


        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context) => _roleManager ?? CreateNew(context);

        private static ApplicationRoleManager CreateNew(IOwinContext context)
        {
            var appRoleManager = new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));
            _roleManager = appRoleManager;
            return appRoleManager;
        }
    }
}