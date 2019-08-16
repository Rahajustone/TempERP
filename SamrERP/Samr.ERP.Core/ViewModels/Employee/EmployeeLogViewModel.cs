using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Employee
{
    public class EmployeeLogViewModel
    {
        public Guid Id { get; set; }
        public string PhotoPath { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public bool HasUser { get; set; }
        public Guid? UserId { get; set; }
        public bool? HasAccount { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Guid EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string DateOfBirth { get; set; }
        public string Description { get; set; }
        public string FactualAddress { get; set; }
        public string GenderName { get; set; }
        public string HireDate { get; set; }
        public bool IsLocked { get; set; }
        public string EmployeeLockReasonName { get; set; }
        public DateTime? LockDate { get; set; }
        public string LockUserFullName { get; set; }
        public string CreatedUserName { get; set; }
    }
}
