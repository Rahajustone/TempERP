using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Account;
using Samr.ERP.Core.ViewModels.Employee;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Interfaces
{
    public interface IEmployeeService
    {
        Task<BaseDataResponse<GetEmployeeViewModel>> GetByIdAsync(Guid id);

        Task<BaseDataResponse<PagedList<AllEmployeeViewModel>>> AllAsync(PagingOptions pagingOptions, FilterEmployeeViewModel filterEmployeeViewModel);
        Task<BaseDataResponse<PagedList<AllLockEmployeeViewModel>>> GetAllLockedEmployeeAsync(PagingOptions pagingOptions, FilterEmployeeViewModel filterEmployeeViewModel);

        Task<BaseDataResponse<EditEmployeeViewModel>> CreateAsync(EditEmployeeViewModel editEmployeeViewModel);
        Task<BaseDataResponse<UserViewModel>> CreateUserForEmployee(Guid employeeId);

        Task<BaseDataResponse<EditEmployeeViewModel>> EditAsync(EditEmployeeViewModel editEmployeeViewModel);

        Task<BaseResponse> EditUserDetailsAsync(
            EditUserDetailsViewModel editUserDetailsView);

        Task<BaseResponse> LockEmployeeAsync(LockEmployeeViewModel lockEmployeeViewModel);
        Task<BaseResponse> UnLockEmployeeAsync(Guid employeeId);
        Task<BaseDataResponse<GetPassportDataEmployeeViewModel>> GetPassportDataAsync(Guid employeeId);
        Task<BaseResponse> EditPassportDataAsync(EditPassportDataEmployeeViewModel editPassportDataEmployeeViewModel);
    }
}
