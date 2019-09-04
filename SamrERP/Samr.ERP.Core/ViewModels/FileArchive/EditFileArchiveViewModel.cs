using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Samr.ERP.Core.Staff;
using Samr.ERP.Core.ViewModels.Employee;

namespace Samr.ERP.Core.ViewModels.FileArchive
{
    public class EditFileArchiveViewModel : FileArchiveViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string CreatedUserName => Extension.FullNameToString(LastName, FirstName, MiddleName);

        public IFormFile File { get; set; }
        public MiniProfileViewModel Author { get; set; }
        public string FileName { get; set; }
    }
}
