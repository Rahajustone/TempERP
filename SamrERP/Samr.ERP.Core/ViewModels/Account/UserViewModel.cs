using System;

namespace Samr.ERP.Core.ViewModels.Account
{
    public class UserViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsLocked { get; set; }
        public Guid? UserLockReasonId { get; set; }
        public string UserLockReasonName { get; set; }
        public DateTime? LockDate { get; set; }
        public string LockUserFullName { get; set; }
        
    }
}
