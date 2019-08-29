using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Handbook.UserLockReason
{
    public class RequestUserLockReasonViewModel
    {
        public Guid Id { get; set; }
        [StringLength(32)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
