using System;
using System.ComponentModel.DataAnnotations;

namespace Samr.ERP.Core.ViewModels.Handbook.UserLockReason
{
    public class UserLockReasonViewModel
    {
        public Guid Id { get; set; }
        [StringLength(32)]
        public string Name { get; set; }
        public string CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public string CreatedUserName { get; set; }
    }
}
