using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Samr.ERP.Infrastructure.Entities.BaseObjects;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    public class SMSMessageHistory:CreatableByUserBaseObject,ICreatable
    {
        public Guid ReceiverUserId { get; set; }
        [ForeignKey(nameof(ReceiverUserId))]
        public User ReceiverUser { get; set; }

        public string PhoneNumber { get; set; }

        public Guid SMPPSettingId { get; set; }
        [ForeignKey(nameof(SMPPSettingId))]
        public SMPPSetting SMPPSetting { get; set; }

        public string Message { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
