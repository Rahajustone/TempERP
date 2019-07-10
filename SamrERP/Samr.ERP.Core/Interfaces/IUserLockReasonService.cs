using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Handbook.UserLockReason;

namespace Samr.ERP.Core.Interfaces
{
    public interface IUserLockReasonService
    {
        Task<BaseDataResponse<UserLockReasonViewModel>> GetByIdAsync(Guid id);
        Task<BaseDataResponse<PagedList<UserLockReasonViewModel>>> GetAllAsync(PagingOptions pagingOptions);
        Task<BaseDataResponse<UserLockReasonViewModel>> CreateAsync(UserLockReasonViewModel userLockReasonViewModel);
        Task<BaseDataResponse<UserLockReasonViewModel>> UpdateAsync(UserLockReasonViewModel userLockReasonViewModel);
    }
}
