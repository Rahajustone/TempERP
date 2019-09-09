using System;
using System.Collections.Generic;
using System.Text;
using Entity = Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.ViewModels.Department
{
    public class DepartmentListViewModel
    {
        public Entity.Department Department { get; set; }
        public DateTime ModifiedAt { get; set; }
        public Entity.User CreatedUser { get; set; }
        public Entity.Employee LastModifiedEmployee { get; set; }

        //public Entity.Employee Employee { get; set; }
        //public DateTime ModifiedAt { get; set; }
    }

    
}
