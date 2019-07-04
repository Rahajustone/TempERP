using System;
using System.ComponentModel.DataAnnotations;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.ViewModels.Employee
{
    public class EditEmployeeViewModel : EmployeeViewModel
    {
        [Required]
        public Guid? GenderId { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public Guid? PositionId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime HireDate { get; set; }

        [StringLength(256, ErrorMessage = "Description length must be not more 256")]
        public string Description { get; set; }

        public string FactualAddress { get; set; }

        public DateTime? LockDate { get; set; }

        public Guid? EmployeeLockReasonId { get; set; }

        public Guid? LockUserId { get; set; }

        public string PassportNumber { get; set; }

        public string PassportIssuer { get; set; }

        public DateTime PassportIssueDate { get; set; }

        public Guid NationalityId { get; set; }

        public string PassportAddress { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }
        public string CreatedUserName { get; set; }

        public string GenderName { get; set; }
        public string PositionName { get; set; }
        public string EmployeeLockReasonName { get; set; }
        public string LockUserName { get; set; }
        public string NationalityName { get; set; }
        public Guid? UserId { get; set; }

    }
}
