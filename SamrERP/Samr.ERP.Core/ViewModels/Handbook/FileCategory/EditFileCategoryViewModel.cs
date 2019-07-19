﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Handbook.FileCategory
{
    public class EditFileCategoryViewModel : FileCategoryViewModel
    {
        public bool IsActive { get; set; }

        public string CreatedUserName { get; set; }

        public string CreatedAt { get; set; }
    }
}