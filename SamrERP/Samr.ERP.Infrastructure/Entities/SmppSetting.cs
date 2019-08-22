using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Infrastructure.Entities.BaseObjects;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    
    public class SMPPSetting : CreatableByUserBaseObject, ICreatable, IActivable
    {
        public string ProviderName { get; set; }
        public string Host { get; set; }
        public int PortNumber { get; set; }
        public string SystemId { get; set; }
        public string Password { get; set; }
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
