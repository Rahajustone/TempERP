using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Employee
{
    public class GetPassportDataEmployeeViewModel
    {
        public string PassportNumber { get; set; }
        public string PassportIssuer { get; set; }
        public DateTime? PassportIssueDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public string PassportAddress { get; set; }
    }
}
