using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Samr.ERP.Core.ViewModels.Account;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.ViewModels.Department
{
    public class DepartmentViewModel
    {
        [Required]
        public string Name { get; set; }

        public Guid RootId;

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
