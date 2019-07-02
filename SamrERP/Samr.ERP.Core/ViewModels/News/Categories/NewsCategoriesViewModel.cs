using System;
using System.ComponentModel.DataAnnotations;

namespace Samr.ERP.Core.ViewModels.News.Categories
{
    public class NewsCategoriesViewModel
    {
        [Required]
        [StringLength(64)]
        public string Name { get; set; }    
        public string CreatedUserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
