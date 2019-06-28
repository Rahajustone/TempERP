using System;
using System.ComponentModel.DataAnnotations;
using Samr.ERP.Infrastructure.Entities.BaseObjects;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    public class Department : CreatableByUserBaseObject, IActivable, ICreatable
    {
        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        public Guid? RootId { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
