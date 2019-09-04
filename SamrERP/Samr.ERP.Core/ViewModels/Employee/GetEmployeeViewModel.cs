using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Employee
{
    public class GetEmployeeViewModel : AllEmployeeViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string DateOfBirth { get; set; }
        public string Description { get; set; }
        public string FactualAddress { get; set; }
        public string GenderName { get; set; }
        public  string HireDate { get; set; }
        public bool IsLocked { get; set; }
        public string EmployeeLockReasonName { get; set; }
        public string LockDate { get; set; }
        public string LockUserFullName { get; set; }
        public Guid GenderId { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid PositionId { get; set; }
        public string PhotoPathMax { get; set; }

        public string CreatedAt { get; set; }
        public string CreateUserFullName { get; set; }
        public string LastEditedAt { get; set; }
    }
}
