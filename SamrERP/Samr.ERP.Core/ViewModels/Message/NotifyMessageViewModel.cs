using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Message
{
    public class NotifyMessageViewModel
    {
        public int TotalUnReadedMessage { get; set; }
        public GetSenderMessageViewModel Message { get; set; }
    }
}
