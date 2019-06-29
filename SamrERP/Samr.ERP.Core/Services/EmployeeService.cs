using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public EmployeeService(
            IUnitOfWork unitOfWork,
            IUserService userService,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _mapper = mapper;
        }
        public async Task<BaseDataResponse<Employee>> CreateAsync(Employee employee)
        {
            _unitOfWork.Employees.Add(employee);

            await _unitOfWork.CommitAsync();

            var response = BaseDataResponse<Employee>.Success(employee);

            return response;
        }

        public async Task<BaseDataResponse<UserViewModel>> CreateUserForEmployee(Guid employeeId)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId);

            if (employee == null)
                return BaseDataResponse<UserViewModel>.NotFound(null);

            var user = new User()
            {
                UserName = employee.Phone,
                Email = employee.Email,
                PhoneNumber = employee.Phone
            };

            var createUserResult = await _userService.CreateAsync(user, PasswordGenerator.GenerateNewPassword());

            if (!createUserResult.Succeeded)
                return BaseDataResponse<UserViewModel>.Fail(null, createUserResult.Errors.ToErrorModels());

            employee.UserId = user.Id;

            await _unitOfWork.CommitAsync();

            return BaseDataResponse<UserViewModel>.Success(_mapper.Map<UserViewModel>(user));

        }

        public async Task UpdateAsync(Employee employee)
        {
            if (employee == null) return;

            User employeeUser;
            if (employee.UserId != null)
            {
                if (employee.User != null) employeeUser = employee.User;
                else
                {
                    employeeUser = await _unitOfWork.Users.GetByIdAsync(employee.UserId.Value);
                }
                employeeUser.Email = employee.Email;
                //TODO need to complete phone changing

                _unitOfWork.Users.Update(employeeUser);

                _unitOfWork.Employees.Update(employee);

                await _unitOfWork.CommitAsync();
            }
        }
        public async Task<BaseResponse> EditUserDetailsAsync(
            EditUserDetailsViewModel editUserDetailsView)
        {
            var userExists = await _unitOfWork.Users.ExistsAsync(editUserDetailsView.UserId);

            if (!userExists)
                return BaseResponse.NotFound();

            var employee = await _unitOfWork.Employees.All().FirstOrDefaultAsync(x => x.UserId == editUserDetailsView.UserId);

            employee.Email = editUserDetailsView.Email;
            employee.AddressFact = editUserDetailsView.AddressFact;

            await UpdateAsync(employee);

            return BaseResponse.Success();
        }


    }
}
