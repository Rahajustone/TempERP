using System;
using System.ComponentModel.DataAnnotations;
using Samr.ERP.Infrastructure.Entities.BaseObjects;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    public class Department : CreatableByUserBaseObject, IActivable, ICreatable
    {
        [Required]
        public string Name { get; set; }

        public Guid RootId;

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
