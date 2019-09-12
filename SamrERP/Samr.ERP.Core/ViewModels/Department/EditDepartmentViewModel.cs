using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Core.Staff;

namespace Samr.ERP.Core.ViewModels.Department
{
    public class EditDepartmentViewModel : DepartmentViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string CreatedUserName => Extension.FullNameToString(LastName, FirstName, MiddleName);

        public string CreatedAt { get; set; }
    }
}
