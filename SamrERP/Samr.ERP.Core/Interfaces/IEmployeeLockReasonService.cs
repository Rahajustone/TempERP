using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.Department;
using Samr.ERP.Core.ViewModels.Handbook;

namespace Samr.ERP.Core.Interfaces
{
    public interface IEmployeeLockReasonService
    {
        Task<BaseResponse<EditEmployeeLockReasonViewModel>> GetByIdAsync(Guid id);
        Task<BaseResponse<IEnumerable<EmployeeLockReasonViewModel>>> GetAll();
        Task<BaseResponse<EditEmployeeLockReasonViewModel>> CreateAsync(EditEmployeeLockReasonViewModel employeeLockReasonViewModel);
        Task<BaseResponse<EditEmployeeLockReasonViewModel>> UpdateAsync(EditEmployeeLockReasonViewModel employeeLockReasonViewModel);
    }
}
