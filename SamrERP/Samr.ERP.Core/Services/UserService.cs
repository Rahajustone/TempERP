using System;
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
using Samr.ERP.Core.ViewModels.Account;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class UserService:IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserService(
            IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<BaseResponse<UserViewModel>> CreateAsync(RegisterUserViewModel registerModel, string password)
        {
            var user = new User()
            {
                UserName = registerModel.Phone,
                Email = registerModel.Email,
                PhoneNumber = registerModel.Phone
            };
            var identityResult = await _userManager.CreateAsync(user, password);

            var response = new BaseResponse<UserViewModel>(_mapper.Map<UserViewModel>(user), identityResult.Succeeded,
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

        public async Task<User> GetUserAsync(ClaimsPrincipal userPrincipal)
        {
            var user = await _unitOfWork.Users.GetDbSet().FirstOrDefaultAsync(p=> p.PhoneNumber == userPrincipal.Identity.Name); // await _userManager.GetUserAsync(userPrincipal);
            return user;
        }

        public IEnumerable<User> GetAllUser()
        {
            var users = _unitOfWork.Users.GetAll().ToList();
            return users;
        }

        public async Task<BaseResponse<string>> ResetPassword(ResetPasswordViewModel resetPasswordModel)
        {
            var user = await GetByPhoneNumber(resetPasswordModel.PhoneNumber);
            if (user == null) return BaseResponse<string>.Fail("",new ErrorModel("user not found"));
            
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, token, "test");

            var response = new BaseResponse<string>(string.Empty, resetPasswordResult.Succeeded,
                resetPasswordResult.Errors.Select(p => new ErrorModel()
                {
                    //Code = //TODO: надо доделать
                    Description = p.Description
                }));
            
            return response;
        }
    }
}
