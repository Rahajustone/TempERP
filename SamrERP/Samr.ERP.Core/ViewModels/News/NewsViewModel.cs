using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.ViewModels.News
{
    public class NewsViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "Must not be more than 64")]
        public string Title { get; set; }
        
        [Required]
        [StringLength(256, ErrorMessage = "Must not be more than 256")]
        public string ShortDescription { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string PublishAt { get; set; }

        public string CreatedAt { get; set; }
    }
}
