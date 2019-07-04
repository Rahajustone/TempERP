using System;

namespace Samr.ERP.Core.ViewModels.Account
{
    public class UserViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
    }
}
