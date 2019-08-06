using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Samr.ERP.Core.ViewModels.UsefulLink
{
    public class UsefulLinkViewModel
    {
        public Guid Id { get; set; }

        public Guid UsefulLinkCategoryId { get; set; }

        public String UsefulLinkCategory { get; set; }

        [Required]
        public string Url { get; set; }
        
        [Required]
        [StringLength(128)]
        public string Title { get; set; }

        [StringLength(512)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
        public string CreatedUserName { get; set; }
    }
}
