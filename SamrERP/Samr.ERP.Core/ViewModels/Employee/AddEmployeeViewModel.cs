using System;
using System.ComponentModel.DataAnnotations;
using Samr.ERP.Core.ViewModels.Common;

namespace Samr.ERP.Core.ViewModels.Employee
{
    public class AddEmployeeViewModel
    {
        [Required]
        [StringLength(32, ErrorMessage = "FistName length must be not more 32")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(32, ErrorMessage = "LastName length must be not more 32")]
        public string LastName { get; set; }

        [StringLength(32, ErrorMessage = "MiddleName length must be not more 32")]
        public string MiddleName { get; set; }

        // TODO make this field unique
        [StringLength(32, ErrorMessage = "PhotoPath length must be not more 32")]
        public string PhotoPath { get; set; }
        
        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        [StringLength(256, ErrorMessage = "Description length must be not more 256")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(128)]
        public string Email { get; set; }

        public DateTime LockDate { get; set; }
            
        public int LockTypeId { get; set; }

        public Guid LockUserId { get; set; }

        public Gender Genre;
    }
}
