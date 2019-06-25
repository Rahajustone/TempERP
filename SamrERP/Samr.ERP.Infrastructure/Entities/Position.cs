using System;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    public class Position : BaseObject, IActivable, ICreatable
    {
        public string Name { get; set; }
        public Guid DepartmentId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreateUserId { get; set; }
    }
}
