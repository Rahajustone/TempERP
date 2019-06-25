using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    public class EmployeeLock: BaseObject, ICreatable, IActivable, ICreatableByUser
    {
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedUserId { get; set; }
    }
}
