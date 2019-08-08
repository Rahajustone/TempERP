﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Handbook.UserLockReason;

namespace Samr.ERP.Core.Interfaces
{
    public interface IUserLockReasonService
    {
        Task<BaseDataResponse<UserLockReasonViewModel>> GetByIdAsync(Guid id);
        Task<BaseDataResponse<PagedList<UserLockReasonViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule);
        Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllListItemsAsync();
        Task<BaseDataResponse<UserLockReasonViewModel>> CreateAsync(UserLockReasonViewModel userLockReasonViewModel);
        Task<BaseDataResponse<UserLockReasonViewModel>> EditAsync(UserLockReasonViewModel userLockReasonViewModel);
        Task<BaseDataResponse<PagedList<UserLockReasonLogViewModel>>> GetAllLogAsync(Guid id, PagingOptions pagingOptions, SortRule sortRule);
    }
}
