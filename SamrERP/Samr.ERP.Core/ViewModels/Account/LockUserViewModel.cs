using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.ViewModels.Account
{
    public class LockUserViewModel
    {
        public Guid Id { get; set; }
        public Guid UserLockReasonId { get; set; }
    }
}
