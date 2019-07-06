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

        [Required]
        [StringLength(9, ErrorMessage = "Phone Number length must not be more than 9 character")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(128)]
        public string Email { get; set; }
    }
}
