using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Notification
{
    public class FilterNotificationViewModel
    {
        public string Title { get; set; }
        public DateTime? FromDate{ get; set; }
        public DateTime? ToDate { get; set; }
    }
}
