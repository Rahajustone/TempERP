﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Account;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly EmployeeService _employeeService;
        private readonly IMapper _mapper;

        public UserService(
            IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            EmployeeService employeeService,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _employeeService = employeeService;
            _mapper = mapper;
        }

        public async Task<IdentityResult> CreateAsync(User user, string password)
        {
            var identityResult = await _userManager.CreateAsync(user, password);

            return identityResult;
        }


        public async Task<User> GetByUserName(string userName)
        {
            var userResult = await _unitOfWork.Users.GetDbSet().FirstOrDefaultAsync(p => p.UserName == userName);
            return userResult;
        }

        public async Task<User> GetByPhoneNumber(string phoneNumber)
        {
            var userResult = await _unitOfWork.Users.GetDbSet().FirstOrDefaultAsync(p => p.PhoneNumber == phoneNumber);
            return userResult;
        }

        public async Task<User> GetUserAsync(ClaimsPrincipal userPrincipal)
        {
            var user = await _unitOfWork.Users.GetDbSet().FirstOrDefaultAsync(p => p.PhoneNumber == userPrincipal.Identity.Name); // await _userManager.GetUserAsync(userPrincipal);
            return user;
        }

        public IEnumerable<User> GetAllUser()
        {
            var users = _unitOfWork.Users.GetAll().ToList();
            return users;
        }

        public async Task<BaseDataResponse<string>> ResetPasswordAsync(ResetPasswordViewModel resetPasswordModel)
        {
            var user = await GetByPhoneNumber(resetPasswordModel.PhoneNumber);
            if (user == null) return BaseDataResponse<string>.Fail("", new ErrorModel("user not found"));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, token, PasswordGenerator.GenerateNewPassword());

            if (!resetPasswordResult.Succeeded)
                return BaseDataResponse<string>.Fail(null, resetPasswordResult.Errors.Select(p => new ErrorModel()
                {
                    //Code = //TODO: надо доделать
                    Description = p.Description
                }).ToArray());

            return BaseDataResponse<string>.Success(null);
        }


        public async Task<BaseDataResponse<string>> ChangePasswordAsync(ChangePasswordViewModel viewModel)
        {
            BaseDataResponse<string> dataResponse;

            var user = await _unitOfWork.Users.GetByIdAsync(viewModel.Id);
            if (user == null) return BaseDataResponse<string>.NotFound("");

            //TODO send smscode confirmation and check

            if (viewModel.SmsConfirmationCode == 1234)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetPasswordResult = await _userManager.ResetPasswordAsync(user, token, viewModel.Password);

                if (resetPasswordResult.Succeeded)
                    dataResponse = BaseDataResponse<string>.Success(null);
                else
                    dataResponse = BaseDataResponse<string>.Fail(null, resetPasswordResult.Errors.Select(p => new ErrorModel()
                    {
                        //Code = //TODO: надо доделать
                        Description = p.Description
                    }).ToArray());
            }
            else dataResponse = BaseDataResponse<string>.Fail(string.Empty);

            return dataResponse;
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

            await _employeeService.Update(employee);
            return  BaseResponse.Success();
        }
    }
}
