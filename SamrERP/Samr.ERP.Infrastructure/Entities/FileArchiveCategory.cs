﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Samr.ERP.Infrastructure.Entities.BaseObjects;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    public class FileArchiveCategory : FileCategoryBaseObject
    {
        public ICollection<FileArchive> FileArchives { get; set; }
    }
}
