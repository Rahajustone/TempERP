using System;
using System.ComponentModel.DataAnnotations;
using Samr.ERP.Infrastructure.Entities.BaseObjects;
using System.Collections.Generic;

using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    public class Department : DepartmentBaseObject
    {
        public ICollection<Position> Positions { get; set; }
    }
}
