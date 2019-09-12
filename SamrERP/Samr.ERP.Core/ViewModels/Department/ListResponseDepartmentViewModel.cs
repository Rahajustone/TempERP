using System.Collections.Generic;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.ViewModels.Department
{
    public class ListResponseDepartmentViewModel : ResponseDepartmentViewModel
    {
        public IEnumerable<DepartmentLog> DepartmentLog { get; set; } 
    }
}
