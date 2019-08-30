using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.EmailSetting
{
    public class EmailMessageHistoryLogViewModel 
    {
        public string ReceiverUser { get; set; }
        public string ReceiverEmail { get; set; }
        public string EmailSettingId { get; set; }
        public string SenderEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string CreatedAt { get; set; }
    }
}
