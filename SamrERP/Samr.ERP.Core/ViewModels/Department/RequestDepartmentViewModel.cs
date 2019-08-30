using System;
using System.ComponentModel.DataAnnotations;

namespace Samr.ERP.Core.ViewModels.Department
{
    public class RequestDepartmentViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public Guid? RootId { get; set; }

        public bool IsActive { get; set; }
    }
}
