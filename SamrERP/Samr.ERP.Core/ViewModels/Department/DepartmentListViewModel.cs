using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Core.Stuff;
using Entity = Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.ViewModels.Department
{
    public class DepartmentListViewModel
    {
        public Entity.Department Department { get; set; }
        public Entity.DepartmentLog DepartmentLog { get; set; }
        //public Entity.Employee Employee { get; set; }
        //public DateTime CreatedAt { get; set; }
    }
}
