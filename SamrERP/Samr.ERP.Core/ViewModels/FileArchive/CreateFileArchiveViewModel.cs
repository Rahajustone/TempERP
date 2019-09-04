using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Samr.ERP.Core.ViewModels.FileArchive
{
    public class CreateFileArchiveViewModel
    {
        public string Title { get; set; }
        public Guid FileCategoryId { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }
        public bool IsActive { get; set; }
    }
}
