using System;

namespace Samr.ERP.Core.ViewModels.Handbook.EmployeeLockReason
{
    public class RequestEmployeeLockReasonViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
