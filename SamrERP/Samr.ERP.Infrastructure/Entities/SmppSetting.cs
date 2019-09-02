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
        public string Host { get; set; }
        [Required]
        public int PortNumber { get; set; }
        [Required]
        public string SystemId { get; set; }
        [Required]
        public string Password { get; set; }
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
