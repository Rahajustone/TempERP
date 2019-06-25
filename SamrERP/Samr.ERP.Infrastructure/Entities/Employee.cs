using System;
using System.ComponentModel.DataAnnotations;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    public class Employee : BaseObject, ICreatable, IActivable
    {
        [Required]
        [StringLength(32, ErrorMessage = "FistName length must be not more 32")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(32, ErrorMessage = "LastName length must be not more 32")]
        public string LastName { get; set; }

        [StringLength(32, ErrorMessage = "MiddleName length must be not more 32")]
        public string MiddleName { get; set; }

        [Required]
        public Guid PositionId { get; set; }
        
        [StringLength(32, ErrorMessage = "PhotoPath length must be not more 32")]
        public string ImageName { get; set; }

        [Required]
        public Guid GenderId { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(9, ErrorMessage = "Phone Number length must not be more than 9 character")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime HireDate { get; set; }

        [StringLength(256, ErrorMessage = "Description length must be not more 256")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(128)]
        public string Email { get; set; }

        public string AddressFact { get; set; }

        public DateTime LockDate { get; set; }

        public int LockTypeId { get; set; }

        public Guid LockUserId { get; set; }

        public string PassportNumber { get; set; }

        public string PassportMvdName { get; set; }

        public DateTime PassportDate { get; set; }

        public Guid PassportNationality { get; set; }

        public string PassportAddress { get; set; }

        public Guid CreateUserId { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        public Gender Gender { get; set; }

        public EmployeeLock EmployeeLock { get; set; }
    }
}
