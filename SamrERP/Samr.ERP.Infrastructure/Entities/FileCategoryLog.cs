using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Infrastructure.Entities.BaseObjects;

namespace Samr.ERP.Infrastructure.Entities
{
    public class FileCategoryLog : FileCategoryBaseObject
    {
        public Guid FileCategoryId { get; set; }
        public FileCategory FileCategory { get; set; }
    }
}
