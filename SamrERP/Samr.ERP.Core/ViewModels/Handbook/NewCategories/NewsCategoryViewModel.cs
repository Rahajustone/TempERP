using System;
using System.ComponentModel.DataAnnotations;

namespace Samr.ERP.Core.ViewModels.Handbook.NewCategories
{
    public class NewsCategoryViewModel
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(64)]
        public string Name { get; set; }    
        public string CreatedUserName { get; set; }
        public string CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
