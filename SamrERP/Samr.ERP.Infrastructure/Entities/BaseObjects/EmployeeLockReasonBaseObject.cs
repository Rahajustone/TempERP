using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities.BaseObjects
{
    public abstract class EmployeeLockReasonBaseObject : CreatableByUserBaseObject, ICreatable, IActivable
    {
        [StringLength(32)]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
