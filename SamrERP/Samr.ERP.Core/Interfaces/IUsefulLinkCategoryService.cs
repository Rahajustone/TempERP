using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Staff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Handbook.UsefulLinkCategory;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Interfaces
{
    public interface IUsefulLinkCategoryService
    {
        Task<BaseDataResponse<ResponseUsefulLinkCategoryViewModel>> GetByIdAsync(Guid id);
        Task<BaseDataResponse<PagedList<ResponseUsefulLinkCategoryViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule);
        Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllSelectListItemAsync();
        Task<BaseDataResponse<ResponseUsefulLinkCategoryViewModel>> CreateAsync(RequestUsefulLinkCategoryViewModel requestUsefulLinkCategoryViewModel);
        Task<BaseDataResponse<ResponseUsefulLinkCategoryViewModel>> EditAsync(RequestUsefulLinkCategoryViewModel requestUsefulLinkCategoryViewModel);
        Task<BaseDataResponse<PagedList<UsefulLinkCategoryLogViewModel>>> GetAllLogAsync(Guid id, PagingOptions pagingOptions, SortRule sortRule);
    }
}
