using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Samr.ERP.Core.ViewModels.Employee;

namespace Samr.ERP.Core.ViewModels.UsefulLink
{
    public class UsefulLinkViewModel
    {
        public Guid Id { get; set; }

        public Guid UsefulLinkCategoryId { get; set; }

        public string UsefulLinkCategoryName { get; set; }

        [Required]
        public string Url { get; set; }
        
        [Required]
        [StringLength(128)]
        public string Title { get; set; }

        [StringLength(512)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public string CreatedAt { get; set; }
        public string CreatedUserName { get; set; }

        public MiniProfileViewModel Author { get; set; }
    }
}
