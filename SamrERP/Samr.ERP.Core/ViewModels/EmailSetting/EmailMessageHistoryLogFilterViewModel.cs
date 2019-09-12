using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.EmailSetting
{
    public class FilterEmailMessageHistoryLogViewModel
    {
        public string ReceiverName { get; set; }
        public string ReceiverEmail { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
