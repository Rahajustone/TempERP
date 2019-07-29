using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Notification
{
    public class NotificationSystemViewModel
    {
        public string Message { get; set; }
        public bool IsViewed { get; set; }

        public Guid? FromUserId { get; set; }
        public Guid? ToUserId { get; set; }

        public Guid? NotificationTypeId { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
