using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Samr.ERP.Core.Staff;

namespace Samr.ERP.Core.ViewModels.Position
{
    public class ResponsePositionViewModel : RequestPositionViewModel
    {
        public string CreatedAt { get; set; }
        public string DepartmentName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public string CreatedUserName => Extension.FullNameToString(LastName, FirstName, MiddleName);
    }
}
