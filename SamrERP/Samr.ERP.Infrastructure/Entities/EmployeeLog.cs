using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Samr.ERP.Infrastructure.Entities.BaseObjects;

namespace Samr.ERP.Infrastructure.Entities
{
    public class EmployeeLog:EmployeeBaseObject
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
