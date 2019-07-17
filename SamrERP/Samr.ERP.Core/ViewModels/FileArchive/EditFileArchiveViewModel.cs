using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Samr.ERP.Core.ViewModels.FileArchive
{
    public class EditFileArchiveViewModel : FileArchiveViewModel
    {
        public IFormFile File { get; set; }
    }
}
