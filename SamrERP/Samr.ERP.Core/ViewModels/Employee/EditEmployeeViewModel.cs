using System;
using System.ComponentModel.DataAnnotations;
using Samr.ERP.Core.ViewModels.Common;

namespace Samr.ERP.Core.ViewModels.Employee
{
    public class EditEmployeeViewModel : EmployeeViewModel
    {
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
