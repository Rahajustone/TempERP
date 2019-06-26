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
        Task<BaseResponse<DepartmentViewModel>> GetByIdAsync(Guid id);
        Task<BaseResponse<IEnumerable<DepartmentViewModel>>> GetAll();
        Task<BaseResponse<EditDepartmentViewModel>> CreateAsync(EditDepartmentViewModel departmentViewModel);

        Task<BaseResponse<EditDepartmentViewModel>> UpdateAsync(EditDepartmentViewModel department);

        Task<BaseResponse<DepartmentViewModel>> DeleteAsync(Guid id);
        //IEnumerable<Department> GetAllUser();
    }
}
