using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.Account;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Interfaces
{
    public interface IEmployeeService
    {
        Task<BaseDataResponse<Employee>> CreateAsync(Employee employee);
        //IEnumerable<Employee> GetAllUser();
        Task<BaseDataResponse<UserViewModel>> CreateUserForEmployee(Guid employeeId);
    }
}
