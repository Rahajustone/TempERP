using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Infrastructure.Entities.BaseObjects;

namespace Samr.ERP.Infrastructure.Entities
{
    public class UserLockReasonLog : UserLockReasonBaseObject
    {
        public Guid UserLockReasonId { get; set; }
        public UserLockReason UserLockReason { get; set; }
    }
}
