using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.FileArchive
{
    public class FileArchiveViewModel
    {
        public Guid FileCategoryId { get; set; }
        
        public string FileName { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }
        public string CreatedAt { get; set; }
    }
}
