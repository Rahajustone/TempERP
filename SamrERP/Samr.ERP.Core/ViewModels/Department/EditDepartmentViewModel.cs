using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Department
{
    public class EditDepartmentViewModel : DepartmentViewModel
    {
        public bool IsActive { get; set; }
        public string CreatedUserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
