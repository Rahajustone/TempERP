using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Infrastructure.Entities
{
    public class Position
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid DepartmentId { get; set; }

    }
}
