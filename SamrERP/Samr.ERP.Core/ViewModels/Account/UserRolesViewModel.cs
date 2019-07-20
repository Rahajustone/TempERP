using System.Collections.Generic;

namespace Samr.ERP.Core.ViewModels.Account
{
    public class UserRolesViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; }
    }

    public class GroupedUserRolesViewModel
    {
        public string GroupName { get; set; }
        public IEnumerable<UserRolesViewModel> Roles { get; set; }
    }
}
