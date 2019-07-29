using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Samr.ERP.Infrastructure.Entities.BaseObjects;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    public class Notification : CreatableByUserBaseObject, ICreatable, IActivable
    {
        public string Message { get; set; }
        public bool IsViewed { get; set; }

        public Guid? FromUserId { get; set; }
        public Guid? ToUserId { get; set; }

        public Guid? NotificationTypeId { get; set; }
        [ForeignKey(nameof(NotificationTypeId))]
        public NotificationType NotificationType { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }

}
