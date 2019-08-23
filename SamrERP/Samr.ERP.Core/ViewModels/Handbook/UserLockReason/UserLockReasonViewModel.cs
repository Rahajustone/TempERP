using System;
using System.ComponentModel.DataAnnotations;
using Samr.ERP.Core.Stuff;

namespace Samr.ERP.Core.ViewModels.Handbook.UserLockReason
{
    public class UserLockReasonViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string CreatedUserName => Extension.FullNameToString(LastName, FirstName, MiddleName);

        public Guid Id { get; set; }
        [StringLength(32)]
        public string Name { get; set; }
        public string CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
