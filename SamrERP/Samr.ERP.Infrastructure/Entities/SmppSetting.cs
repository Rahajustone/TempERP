using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Infrastructure.Entities.BaseObjects;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    public class SMPPSetting : CreatableByUserBaseObject, ICreatable, IActivable
    {
        public string MessageConnectionName { get; set; }
        public string AddressAttributeName { get; set; }
        public string ServerIpAddress { get; set; }
        public int PortNumber { get; set; }
        public string SenderAddress { get; set; }
        public int SystemId { get; set; }
        public string SystemPassword { get; set; }
        public string SystemType { get; set; }
        public int KeepAliveInterval { get; set; }
        public int TimeOut { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
