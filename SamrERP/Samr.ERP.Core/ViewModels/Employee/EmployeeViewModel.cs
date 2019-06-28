using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.ViewModels.Employee
{
    public class EmployeeViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(32, ErrorMessage = "FistName length must be not more 32")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(32, ErrorMessage = "LastName length must be not more 32")]
        public string LastName { get; set; }

        [StringLength(32, ErrorMessage = "MiddleName length must be not more 32")]
        public string MiddleName { get; set; }

        [StringLength(32, ErrorMessage = "PhotoPath length must be not more 32")]
        public string ImageName { get; set; }

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

        public string PassportNumber { get; set; }

        public string PassportMvdName { get; set; }

        public DateTime PassportDate { get; set; }

        public string PassportAddress { get; set; }
    }
}
