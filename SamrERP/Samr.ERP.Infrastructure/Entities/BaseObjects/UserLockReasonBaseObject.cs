using System;
using System.ComponentModel.DataAnnotations;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities.BaseObjects
{
    public class UserLockReasonBaseObject : CreatableByUserBaseObject, ICreatable, IActivable
    {
        [StringLength(32)]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
