using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.UsefulLink
{
    public class FilterUsefulLinkViewModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public Guid CategoryId { get; set; }
        public string Title { get; set; }
        public bool OnlyActive { get; set; }
    }
}
