using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.Account;
using Samr.ERP.Core.ViewModels.Handbook.UserLockReason;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> CreateAsync(User user, string password);
        Task<User> GetByUserName(string userName);
        Task<User> GetByPhoneNumber(string phoneNumber);
        Task<User> GetUserAsync(ClaimsPrincipal userPrincipal);
        IEnumerable<User> GetAllUser();
        Task<BaseDataResponse<string>> ResetPasswordAsync(ResetPasswordViewModel resetPasswordModel);
        Task<BaseDataResponse<string>> ChangePasswordAsync(ChangePasswordViewModel viewModel);

        Task<BaseResponse> UserLockAsync(LockUserViewModel userLockReasonViewModel);
        Task<BaseResponse> UserUnlockAsync(Guid id);
        Task<BaseDataResponse<UserViewModel>> GetByIdAsync(Guid id);
        bool HasUserValidRefreshToken(Guid userId,string refreshToken, string ipAddress);
        Task AddRefreshToken(string token, Guid userId, string remoteIpAddress, double daysToExpire = 5);
        Task RemoveRefreshToken(Guid userId, string refreshToken);
    }
}