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
    public class FileArchive : CreatableByUserBaseObject, IActivable, ICreatable
    {
        public Guid FileCategoryId { get; set; }
        [ForeignKey(nameof(FileCategoryId))]
        public FileArchiveCategory FileArchiveCategory { get; set; }

        [Required]
        [StringLength(64)]
        public string FilePath { get; set; }

        [Required]
        [StringLength(256)]
        public string Title { get; set; }

        [Required]
        [StringLength(512)]
        public string Description { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
