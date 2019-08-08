using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Samr.ERP.Core.ViewModels.Employee;

namespace Samr.ERP.Core.ViewModels.FileArchive
{
    public class EditFileArchiveViewModel : FileArchiveViewModel
    {
        public IFormFile File { get; set; }
        public string CreatedUserName { get; set; }

        public MiniProfileViewModel Author { get; set; }
    }
}
