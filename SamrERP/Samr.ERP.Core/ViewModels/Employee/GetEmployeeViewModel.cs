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
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public string DateOfBirth { get; set; }
        public string Description { get; set; }
        public string FactualAddress { get; set; }
        public string GenderName { get; set; }
        public  string HireDate { get; set; }
        public bool IsLocked { get; set; }
        public string LockReasonName { get; set; }
        public DateTime? LockDate { get; set; }
        public Guid GenderId { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid PositionId { get; set; }

    }
}
