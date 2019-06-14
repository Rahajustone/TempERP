using Microsoft.AspNet.Identity.EntityFramework;

namespace Samr.ERP.WebApi.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private static ApplicationDbContext applicationDbContext;
        public ApplicationDbContext()
            : base("IdentityConnection", throwIfV1Schema: false)
        {
            //Database.SetInitializer<ApplicationDbContext>(null);
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static ApplicationDbContext Create() =>  applicationDbContext ?? CreateNew();

        private static ApplicationDbContext CreateNew()
        {
            return new ApplicationDbContext();
        }
    }
}