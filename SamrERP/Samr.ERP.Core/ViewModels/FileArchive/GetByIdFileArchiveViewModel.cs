using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.FileArchive
{
    public class GetByIdFileArchiveViewModel : FileArchiveViewModel
    {
        public string FilePath { get; set; }
        public string CreatedAt { get; set; }
        public string FileCategoryName { get; set; }
        public string FileName { get; set; }
    }
}
