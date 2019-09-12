using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Core.ViewModels.Common;

namespace Samr.ERP.Core.ViewModels.Department
{
    public class GetAllDepartmentViewModel
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public bool IsActive { get; set; }
        public string ModifiedAt { get; set; }
        public string LastModifiedAuthor { get; set; }
        public Infrastructure.Entities.Employee Employee { get; set; }

    }
}
