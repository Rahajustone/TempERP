using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Core.Staff;

namespace Samr.ERP.Core.ViewModels.Position
{
    public class PositionLogViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? DepartmentId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedAt { get; set; }
        public string DepartmentName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string CreatedUserName => Extension.FullNameToString(LastName, FirstName, MiddleName);
    }
}
