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
        public Guid? ReceiverUserId { get; set; }
        public DateTime? ReadDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
