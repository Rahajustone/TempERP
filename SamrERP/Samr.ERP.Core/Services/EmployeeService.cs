﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Account;
using Samr.ERP.Core.ViewModels.Employee;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Infrastructure.Data;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.Infrastructure.Providers;

namespace Samr.ERP.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly UserProvider _userProvider;
        private readonly IMapper _mapper;
        private readonly IUploadFileService _file;

        public EmployeeService(
            IUnitOfWork unitOfWork,
            IUserService userService,
            UserProvider userProvider,
            IMapper mapper,
            IUploadFileService file
            )
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _userProvider = userProvider;
            _mapper = mapper;
            _file = file;
        }

        public async Task<BaseDataResponse<EditEmployeeViewModel>> CreateAsync(EditEmployeeViewModel editEmployeeViewModel, IFormFile filePath)
        {
            BaseDataResponse<EditEmployeeViewModel> dataResponse;

            // TODO raha
            //var filePathName = await _file.StorePhoto("wwwroot/employers", filePath);
            //dataResponse = BaseDataResponse<EditEmployeeViewModel>.Success(editEmployeeViewModel);

            var employeeExists = _unitOfWork.Employees.Any(predicate: e =>
                e.Phone.ToLower() == editEmployeeViewModel.Phone.ToLower() &&
                e.PassportNumber.ToLower() == editEmployeeViewModel.PassportNumber.ToLower());

            if (employeeExists)
            {
                dataResponse = BaseDataResponse<EditEmployeeViewModel>.Fail(editEmployeeViewModel, new ErrorModel("Phone number or Passport already exist"));

            }
            else
            {

                var employee = _mapper.Map<Employee>(editEmployeeViewModel);
                _unitOfWork.Employees.Add(employee);
                await _unitOfWork.CommitAsync();

                dataResponse = BaseDataResponse<EditEmployeeViewModel>.Success(_mapper.Map<EditEmployeeViewModel>(employee));
            }

            return dataResponse;
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
            if (employee?.UserId != null)
            {
                User employeeUser;
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

        public async Task<BaseResponse> LockEmployeeAsync(LockEmployeeViewModel lockEmployeeViewModel)
        {
            var employee = await _unitOfWork.Employees.GetDbSet()
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == lockEmployeeViewModel.EmployeeId);

            var employeeLockReason =
                await _unitOfWork.EmployeeLockReasons.GetByIdAsync(lockEmployeeViewModel.EmployeeLockReasonId);

            if (employee == null
                || employeeLockReason == null
                || !employeeLockReason.IsActive
                || employee.EmployeeLockReasonId != null) return BaseResponse.NotFound();

            employee.LockUserId = _userProvider.CurrentUser.Id;
            employee.EmployeeLockReasonId = employeeLockReason.Id;
            employee.LockDate = DateTime.Now;

            await _unitOfWork.CommitAsync();

            return BaseResponse.Success();
        }

        public async Task<BaseResponse> UnLockEmployeeAsync(Guid employeeId)
        {
            var employee = await _unitOfWork.Employees.GetDbSet()
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == employeeId);

            if (employee == null || employee.EmployeeLockReasonId != null) return BaseResponse.NotFound();

            employee.LockUserId = null;
            employee.EmployeeLockReasonId = null;
            employee.LockDate = null;

            await _unitOfWork.CommitAsync();

            return BaseResponse.Success();
        }


    }
}
