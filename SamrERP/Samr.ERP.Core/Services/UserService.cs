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
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Account;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.Infrastructure.Providers;

namespace Samr.ERP.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;

        public UserService(
            IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            IEmailSender emailSender,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailSender = emailSender;
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

        public async Task<BaseDataResponse<UserViewModel>> GetByIdAsync(Guid id)
        {
            var userResult = await _unitOfWork.Users.GetDbSet().Include(p => p.UserLockReason).FirstOrDefaultAsync(p => p.Id == id);
            if (userResult == null) return BaseDataResponse<UserViewModel>.NotFound(null);
            return BaseDataResponse<UserViewModel>.Success(_mapper.Map<UserViewModel>(userResult)); ;
        }

        public bool HasUserValidRefreshToken(Guid userId, string refreshToken, string ipAddress)
        {
            return _unitOfWork.RefreshTokens.Any(p => p.UserId == userId && p.Active && p.RemoteIpAddress == ipAddress);
        }

        public async Task AddRefreshToken(string token, Guid userId, string remoteIpAddress, double daysToExpire = 5)
        {
            var refreshToken = new RefreshToken()
            {
                Token = token,
                Expires = DateTime.Now.AddDays(daysToExpire),
                UserId = userId,
                RemoteIpAddress = remoteIpAddress
            };

            _unitOfWork.RefreshTokens.Add(refreshToken);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveRefreshToken(Guid userId, string token)
        {
            var refreshToken = await _unitOfWork.RefreshTokens.All()
                .FirstOrDefaultAsync(p => p.UserId == userId && p.Token == token);

            if (refreshToken != null)
            {
                _unitOfWork.RefreshTokens.Delete(refreshToken);
                await _unitOfWork.CommitAsync(); 
            }
        }

        public async Task<User> GetByPhoneNumber(string phoneNumber)
        {
            var userResult = await _unitOfWork.Users.GetDbSet().FirstOrDefaultAsync(p => p.PhoneNumber == phoneNumber);
            return userResult;
        }

        public async Task<User> GetUserAsync(ClaimsPrincipal userPrincipal)
        {
            if (userPrincipal == null) return null;
            Guid.TryParse(userPrincipal.Claims.FirstOrDefault(c => c.Type == "id")?.Value, out Guid id);

            var user = await _unitOfWork.Users.GetDbSet().FirstOrDefaultAsync(p => p.Id == id); // await _userManager.GetUserAsync(userPrincipal);
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

            var generatedPass = RandomGenerator.GenerateNewPassword();
            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, token, generatedPass);

            await _emailSender.SendEmailToEmployeeAsync(user, "Reset password", $"Your account pass was reset, new pass {generatedPass}");

            if (!resetPasswordResult.Succeeded)
                return BaseDataResponse<string>.Fail(null, resetPasswordResult.Errors.Select(p => new ErrorModel()
                {
                    //Code = //TODO: надо доделать
                    Description = p.Description
                }).ToArray());

            return BaseDataResponse<string>.Success(generatedPass);
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

        public async Task<BaseResponse> UserLockAsync(LockUserViewModel lockUserViewModel)
        {
            var userExists = await _unitOfWork
                .Users.GetDbSet()
                .FirstOrDefaultAsync(u => u.Id == lockUserViewModel.Id);

            var userLockReasonExists = await _unitOfWork
                .UserLockReasons.GetDbSet()
                .FirstOrDefaultAsync(u => u.Id == lockUserViewModel.UserLockReasonId);

            if (userExists == null
                || userLockReasonExists == null
                || userExists.UserLockReasonId != null)
                return BaseResponse.Fail();

            userExists.UserLockReasonId = lockUserViewModel.UserLockReasonId;
            userExists.LockDate = DateTime.Now;

            await _unitOfWork.CommitAsync();

            return BaseResponse.Success();
        }

        public async Task<BaseResponse> UserUnlockAsync(Guid id)
        {
            var userExists = await _unitOfWork
                .Users.GetDbSet()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (userExists?.UserLockReasonId == null)
                return BaseResponse.Fail();

            userExists.UserLockReasonId = null;
            userExists.LockDate = null;

            await _unitOfWork.CommitAsync();

            return BaseResponse.Success();
        }
    }
}
