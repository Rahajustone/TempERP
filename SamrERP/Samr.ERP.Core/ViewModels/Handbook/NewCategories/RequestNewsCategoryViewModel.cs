using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Handbook.NewCategories
{
    public class RequestNewsCategoryViewModel
    {

        public Guid Id { get; set; }
        [Required]
        [StringLength(64)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
