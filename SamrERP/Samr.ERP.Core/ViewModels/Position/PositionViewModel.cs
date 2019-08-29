using System;
using System.ComponentModel.DataAnnotations;

namespace Samr.ERP.Core.ViewModels.Position
{
    public class PositionViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
