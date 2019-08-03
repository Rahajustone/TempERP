using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Account
{
    public class GetEmployeeDataViewModel
    {
        public string FullName { get; set; }
        public string DateOfBirth { get; set; }
        public string Description { get; set; }
        public string FactualAddress { get; set; }
        public string GenderName { get; set; }
        public string HireDate { get; set; }
        public string DepartmentName { get; set; }
        public string PositionName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PhotoPath { get; set; }
        public string PhotoPathResized { get; set; }
    }
}
