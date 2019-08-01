using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Notification
{
    public class CreateMessageViewModel
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
        public bool IsActive { get; set; }
    }
}
