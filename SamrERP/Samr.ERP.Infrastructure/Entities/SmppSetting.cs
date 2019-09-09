using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Samr.ERP.Infrastructure.Entities.BaseObjects;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    
    public class SMPPSetting : CreatableByUserBaseObject, ICreatable, IActivable
    {
        [Required]
        public string ProviderName { get; set; }
        [Required]
        public string HostName { get; set; }
        [Required]
        public int PortNumber { get; set; }
        [Required]
        public string  UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string SystemType { get; set; }
        public int SourceAddressTon { get; set; }
        public int SourceAddressNpi { get; set; }
        public bool SourceAddressAutodetect { get; set; }
        public int DestAddressTon { get; set; }
        public int DestAddressNpi { get; set; }
        public string InterfaceVersion { get; set; }
        public string DeliveryUserAckRequest { get; set; }
        public bool IntermediateNotification { get; set; }
        public string DataEncoding { get; set; }

        public int ValidityPeriod { get; set; }

        public int TransceiverMode { get; set; }
        public int ReceivePort { get; set; }
        public int EnquireLinkInterval { get; set; }
        public int WaitAckExpire { get; set; }
        public int MaxPendingSubmits { get; set; }
        public int Throughput { get; set; }

        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
