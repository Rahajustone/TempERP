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
        Task<BaseDataResponse<EditEmployeeLockReasonViewModel>> GetByIdAsync(Guid id);
        Task<BaseDataResponse<IEnumerable<EmployeeLockReasonViewModel>>> GetAll();
        Task<BaseDataResponse<EditEmployeeLockReasonViewModel>> CreateAsync(EditEmployeeLockReasonViewModel employeeLockReasonViewModel);
        Task<BaseDataResponse<EditEmployeeLockReasonViewModel>> UpdateAsync(EditEmployeeLockReasonViewModel employeeLockReasonViewModel);
    }
}
