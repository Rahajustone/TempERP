using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
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
        public DateTime HireDate { get; set; }

        [StringLength(256, ErrorMessage = "Description length must be not more 256")]
        public string Description { get; set; }

        public string FactualAddress { get; set; }

        public IFormFile Photo { get; set; }
    }
}
