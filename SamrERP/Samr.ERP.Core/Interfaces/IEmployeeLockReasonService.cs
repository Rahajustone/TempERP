using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Staff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Department;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Handbook.EmployeeLockReason;

namespace Samr.ERP.Core.Interfaces
{
    public interface IEmployeeLockReasonService
    {
        Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllListItemAsync();
        Task<BaseDataResponse<ResponseEmployeeLockReasonViewModel>> GetByIdAsync(Guid id);
        Task<BaseDataResponse<PagedList<ResponseEmployeeLockReasonViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule);
        Task<BaseDataResponse<ResponseEmployeeLockReasonViewModel>> CreateAsync(RequestEmployeeLockReasonViewModel employeeLockReasonViewModel);
        Task<BaseDataResponse<ResponseEmployeeLockReasonViewModel>> EditAsync(RequestEmployeeLockReasonViewModel employeeLockReasonViewModel);
        Task<BaseDataResponse<PagedList<EmployeeLockReasonLogViewModel>>> GetAllLogAsync(Guid id, PagingOptions pagingOptions, SortRule sortRule);
    }
}
