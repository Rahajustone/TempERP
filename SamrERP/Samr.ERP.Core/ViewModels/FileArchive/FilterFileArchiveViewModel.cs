﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.FileArchive
{
    public class FilterFileArchiveViewModel
    {
        public Guid FileCategoryId { get; set; }

        public string ShortDescription { get; set; }

        public bool IsActive { get; set; }
    }
}