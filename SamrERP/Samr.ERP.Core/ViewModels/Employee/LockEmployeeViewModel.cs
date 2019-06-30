using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Employee
{
    public class LockEmployeeViewModel
    {
        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public Guid EmployeeLockReasonId { get; set; }
    }
}
