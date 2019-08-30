using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Samr.ERP.Core.Stuff;

namespace Samr.ERP.Core.ViewModels.Position
{
    public class RequestPositionViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public Guid? DepartmentId { get; set; }
        public bool IsActive { get; set; }
    }
}
