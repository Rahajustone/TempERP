using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Infrastructure.Entities.BaseObjects;

namespace Samr.ERP.Infrastructure.Entities
{
    public class EmployeeLockReasonLog : EmployeeLockReasonBaseObject
    {
        public Guid EmployeeLockReasonId { get; set; }
        public EmployeeLockReason EmployeeLockReason { get; set; }
    }
}
