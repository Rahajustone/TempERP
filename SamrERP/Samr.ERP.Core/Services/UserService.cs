﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class UserService:IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public UserService(IUnitOfWork unitOfWork,UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<BaseResponse<User>> CreateAsync(User user, string password)
        {
            var identityResult = await _userManager.CreateAsync(user, password);

            var response = new BaseResponse<User>(user, identityResult.Succeeded,
                identityResult.Errors.Select(p => new ErrorModel()
                {
                    //Code = //TODO: надо доделать
                    Description = p.Description
                }));

            return response;
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

        public async Task<User> GetCurrentUserAsync(ClaimsPrincipal userPrincipal)
        {
            var user = await _unitOfWork.Users.GetDbSet().FirstOrDefaultAsync(p=> p.PhoneNumber == userPrincipal.Identity.Name); // await _userManager.GetUserAsync(userPrincipal);
            return user;
        }

        public IEnumerable<User> GetAllUser()
        {
            var users = _unitOfWork.Users.GetAll().ToList();
            return users;
        }
    }
}
