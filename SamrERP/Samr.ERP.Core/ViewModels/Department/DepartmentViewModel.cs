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
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public Guid? RootId { get; set; }
    }
}
