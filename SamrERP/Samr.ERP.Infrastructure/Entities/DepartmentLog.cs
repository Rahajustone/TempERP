using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Samr.ERP.Infrastructure.Entities.BaseObjects;

namespace Samr.ERP.Infrastructure.Entities
{
    public class DepartmentLog : DepartmentBaseObject
    {
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
