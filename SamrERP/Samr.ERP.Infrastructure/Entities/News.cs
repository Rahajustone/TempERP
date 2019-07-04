using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Samr.ERP.Infrastructure.Entities.BaseObjects;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    public class News : CreatableByUserBaseObject, ICreatable
    {
        public Guid CatId { get; set; }

        [Required]
        [StringLength(64)]
        public string Title { get; set; }

        [Required]
        [StringLength(256)]
        public string ShortDescription { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public  string PublishAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}











