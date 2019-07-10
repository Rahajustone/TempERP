using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Employee
{
    public class EditPassportDataEmployeeViewModel
    {
        public Guid EmployeeId { get; set; }
        public string PassportNumber { get; set; }
        public string PassportIssuer { get; set; }
        public DateTime? PassportIssueDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Guid NationalityId { get; set; }
        public string PassportAddress { get; set; }
    }
}
