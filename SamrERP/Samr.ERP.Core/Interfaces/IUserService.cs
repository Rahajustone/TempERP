﻿using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.Account;
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
    }
}