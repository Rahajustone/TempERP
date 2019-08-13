using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Core.ViewModels.Employee;

namespace Samr.ERP.Core.ViewModels.Notification
{
    public class NotificationSystemViewModel : CreateMessageViewModel
    {
        public Guid Id { get; set; }
        public Guid? SenderUserId { get; set; }
        public string ReadDate { get; set; }
        public string CreatedAt { get; set; }

        public MiniProfileViewModel User { get; set; }
    }
}
