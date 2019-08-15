using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.Account;
using Samr.ERP.Core.ViewModels.EmailSetting;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Interfaces
{
    public interface IEmailSettingService
    {
        EmailSetting GetDefaultEmailSetting();
        Task<BaseDataResponse<EmailSettingViewModel>> CreateAsync(EmailSettingViewModel emailSettingView);
        Task<BaseDataResponse<EmailSettingViewModel>> GetByIdAsync(Guid id);
        BaseDataResponse<IEnumerable<EmailSettingViewModel>> GetAll();
        Task<BaseResponse> Remove(Guid id);
    }
}