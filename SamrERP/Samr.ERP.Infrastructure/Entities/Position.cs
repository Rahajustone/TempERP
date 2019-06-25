using System;

namespace Samr.ERP.Infrastructure.Entities
{
    public class Position : BaseObject
    {
        public string Name { get; set; }
        public Guid DepartmentId { get; set; }

    }
}
