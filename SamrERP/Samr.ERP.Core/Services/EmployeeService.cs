using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Account;
using Samr.ERP.Infrastructure.Data;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserService _userService;

        public EmployeeService(
            IUnitOfWork unitOfWork,
            UserService userService
            )
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }
        public async Task<BaseResponse<Employee>> CreateAsync(Employee employee)
        {
            _unitOfWork.Employees.Add(employee);

            await _unitOfWork.CommitAsync();

            var response = BaseResponse<Employee>.Success(employee);

            return response;
        }

        public async Task<BaseResponse<Employee>> CreateUserForEmployee(Guid employeeId)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId);
            
            if (employee == null)
                return BaseResponse<Employee>.NotFound(null);
            var user = new User()
            {
                UserName = employee.Phone,
                Email = employee.Email,
                PhoneNumber = employee.Phone
            };

            var createUserResult = await _userService.CreateAsync(user, PasswordGenerator.GenerateNewPassword());

            if (!createUserResult.Succeeded)
                return BaseResponse<Employee>.Fail(employee, createUserResult.Errors.ToErrorModels());

            employee.UserId = user.Id;

            await _unitOfWork.CommitAsync();

            return  BaseResponse<Employee>.Success(employee);

        }

    }
}
