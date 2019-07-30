using System.Collections.Generic;

namespace Samr.ERP.Core.ViewModels.Account
{
    public class GroupedUserRolesViewModel
    {
        public string GroupName { get; set; }
        public IEnumerable<UserRolesViewModel> Roles { get; set; }
    }
}