using System;

namespace Samr.ERP.Core.ViewModels.Message
{
    public class FilterMessageViewModel
    {
        public string Title { get; set; }
        public DateTime? FromDate{ get; set; }
        public DateTime? ToDate { get; set; }
    }
}
