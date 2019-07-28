using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Samr.ERP.Infrastructure.Entities.BaseObjects;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    public class EmailSetting:CreatableByUserBaseObject,ICreatable,IActivable
    {
        [Required]
        [StringLength(128)]
        public string MailServerName { get; set; }

        [Required]
        [StringLength(128)]
        public string MailServer { get; set; }

        [Required]
        [Range(1, 65535)]
        public ushort MailPort { get; set; }

        [Required]
        public bool EnabledSSL { get; set; }

        [StringLength(128)]
        public string SenderName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Sender { get; set; }

        public string Password { get; set; }

        public bool IsDefault { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }
    }
}
