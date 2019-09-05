using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Core.Staff;
using Samr.ERP.Core.ViewModels.Employee;

namespace Samr.ERP.Core.ViewModels.FileArchive
{
    public class GetListFileArchiveViewModel : GetByIdFileArchiveViewModel
    {
        public MiniProfileViewModel Author { get; set; }
    }
}
