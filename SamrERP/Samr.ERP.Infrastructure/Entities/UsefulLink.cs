using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Samr.ERP.Infrastructure.Entities.BaseObjects;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    public class UsefulLink : CreatableByUserBaseObject, IActivable, ICreatable
    {
        [Required]
        public Guid UsefulLinkCategoryId { get; set; }
        public UsefulLinkCategory UsefulLinkCategory { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        [StringLength(128)]
        public string Title { get; set; }
        
        [StringLength(512)]
        public string Description { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
