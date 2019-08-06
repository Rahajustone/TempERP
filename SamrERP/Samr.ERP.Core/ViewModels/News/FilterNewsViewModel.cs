using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.News
{
    public class FilterNewsViewModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public Guid CategoryId { get; set; }
        public string Title { get; set; }
        public bool OnlyActive { get; set; }
    }
}
