using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.FileArchive
{
    public class FilterFileArchiveViewModel
    {
        public Guid CategoryId { get; set; }

        public string Title { get; set; }

        public bool OnlyActive { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
