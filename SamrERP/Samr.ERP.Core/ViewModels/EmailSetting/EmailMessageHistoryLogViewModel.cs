using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.EmailSetting
{
    public class EmailMessageHistoryLogViewModel 
    {
        public string RecieverUser { get; set; }
        public string RecieverEMail { get; set; }
        public string EmailSetting { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string CreatedAt { get; set; }
    }
}
