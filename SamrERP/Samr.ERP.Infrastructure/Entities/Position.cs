using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Samr.ERP.Infrastructure.Entities.BaseObjects;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    public class Position : BaseObject, IActivable, ICreatable
    {
        public string Name { get; set; }

        public Guid DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

 
    }
}
