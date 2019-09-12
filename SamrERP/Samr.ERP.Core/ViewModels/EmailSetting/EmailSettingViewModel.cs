using System;
using System.ComponentModel.DataAnnotations;

namespace Samr.ERP.Core.ViewModels.EmailSetting
{
    public class EmailSettingViewModel
    {
        public Guid? Id { get; set; }
        [Required]
        [StringLength(128)]
        public string MailServerName { get; set; }
        [Required]
        [StringLength(128)]
        public string MailServer { get; set; }
        [Required]
        [Range(1, 65535)]
        public ushort MailPort { get; set; }
        [StringLength(128)]
        public string SenderName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Sender { get; set; }
        public string Password { get; set; }
        public bool IsDefault { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool SSL { get; set; }
    }
}
