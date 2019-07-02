using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.Account;
using Samr.ERP.Core.ViewModels.Employee;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Interfaces
{
    public interface IEmployeeService
    {
        Task<BaseDataResponse<GetEmployeeViewModel>> GetByIdAsync(Guid id);
        Task<BaseDataResponse<IEnumerable<AllEmployeeViewModel>>> AllAsync();
        Task<BaseDataResponse<EditEmployeeViewModel>> CreateAsync(EditEmployeeViewModel editEmployeeViewModel);
        Task<BaseDataResponse<UserViewModel>> CreateUserForEmployee(Guid employeeId);

        Task<BaseDataResponse<EditEmployeeViewModel>> UpdateAsync(EditEmployeeViewModel editEmployeeViewModel);

        Task<BaseResponse> EditUserDetailsAsync(
            EditUserDetailsViewModel editUserDetailsView);

        Task<BaseResponse> LockEmployeeAsync(LockEmployeeViewModel lockEmployeeViewModel);
        Task<BaseResponse> UnLockEmployeeAsync(Guid employeeId);
    }
}
