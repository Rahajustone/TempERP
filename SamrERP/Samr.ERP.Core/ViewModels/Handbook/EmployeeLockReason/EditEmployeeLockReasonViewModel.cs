using System;

namespace Samr.ERP.Core.ViewModels.Handbook.EmployeeLockReason
{
    public class EditEmployeeLockReasonViewModel : EmployeeLockReasonViewModel
    {
        public bool IsActive { get; set; }

        public string CreatedUserName { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
