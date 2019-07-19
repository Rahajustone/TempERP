using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Account
{
    public class SetUserRolesViewModel
    {
        public Guid UserId { get; set; }
        public Guid[] RolesId { get; set; }
    }
}
