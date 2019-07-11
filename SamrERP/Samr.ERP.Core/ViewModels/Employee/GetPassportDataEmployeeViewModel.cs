using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Employee
{
    public class GetPassportDataEmployeeViewModel
    {
        public string PassportNumber { get; set; }
        public string PassportIssuer { get; set; }
        public string PassportIssueDate { get; set; }
        public string DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public Guid NationalityId { get; set; }
        public string PassportAddress { get; set; }
        public string PassportScanPath { get; set; }
    }
}
