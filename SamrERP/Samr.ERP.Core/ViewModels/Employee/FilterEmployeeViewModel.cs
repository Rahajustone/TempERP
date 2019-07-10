using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Employee
{
    public class FilterEmployeeViewModel
    {
        public string FullName { get; set; }
        public Guid? DepartmentId { get; set; }
        public bool HasUser { get; set; }
    }
}
