using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.FileArchive
{
    public class FileArchiveViewModel
    {
        public Guid Id { get; set; }
        public Guid FileCategoryId { get; set; }
        public string FileCategoryName { get; set; }

        public string FilePath { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }
        public string CreatedAt { get; set; }
    }
}
