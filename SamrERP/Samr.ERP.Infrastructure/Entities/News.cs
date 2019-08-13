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
    public class News : CreatableByUserBaseObject, ICreatable, IActivable
    {
        public Guid NewsCategoryId { get; set; }
        [ForeignKey(nameof(NewsCategoryId))]
        public NewsCategory NewsCategory { get; set; }

        [Required]
        [StringLength(256)]
        public string Title { get; set; }

        [Required]
        [StringLength(256)]
        public string ShortDescription { get; set; }

        [Required]
        public string Description { get; set; }
        public DateTime PublishAt { get; set; }

        public string Image { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}











