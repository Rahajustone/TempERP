using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.FileArchive
{
    public class FileArchiveViewModel
    {
        public string Title { get; set; }
        public Guid FileCategoryId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
