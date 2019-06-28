using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.Department;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Interfaces
{
    public interface IDepartmentService
    {
        Task<BaseDataResponse<EditDepartmentViewModel>> GetByIdAsync(Guid id);
        Task<BaseDataResponse<IEnumerable<DepartmentViewModel>>> GetAllAsync();
        Task<BaseDataResponse<EditDepartmentViewModel>> CreateAsync(EditDepartmentViewModel departmentViewModel);
        Task<BaseDataResponse<EditDepartmentViewModel>> UpdateAsync(EditDepartmentViewModel departmentViewModel);
    }
}
