using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Employee
{
    public class ExportExcelViewModel
    {
        public string FullName { get; set; }
        public string DepartmentName { get; set; }
        public string PositionName { get; set; }
        public string HasAccount { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
