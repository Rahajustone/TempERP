using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Samr.ERP.Infrastructure.Entities.BaseObjects;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    public class Notification : CreatableByUserBaseObject, ICreatable
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public Guid? SenderUserId { get; set; }
        [ForeignKey(nameof(SenderUserId))]
        public User SenderUser { get; set; }
        public DateTime? ReadDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? ReceiverUserId { get; set; }
        [ForeignKey(nameof(ReceiverUserId))]
        public User ReceiverUser { get; set; }
    }
}
