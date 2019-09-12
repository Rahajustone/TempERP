using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Infrastructure.Entities.BaseObjects;

namespace Samr.ERP.Infrastructure.Entities
{
    public class FileArchiveCategoryLog : FileCategoryBaseObject
    {
        public Guid FileCategoryId { get; set; }
        public FileArchiveCategory FileArchiveCategory { get; set; }
    }
}
