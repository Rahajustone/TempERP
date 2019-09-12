using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Staff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Handbook.NewCategories;

namespace Samr.ERP.Core.Interfaces
{
    public interface INewsCategoryService
    {
        Task<BaseDataResponse<ResponseNewsCategoryViewModel>> GetByIdAsync(Guid id);
        Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllSelectListItemAsync();
        Task<BaseDataResponse<PagedList<ResponseNewsCategoryViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule);
        Task<BaseDataResponse<ResponseNewsCategoryViewModel>> CreateAsync(RequestNewsCategoryViewModel requestNewsCategoryViewModel);
        Task<BaseDataResponse<ResponseNewsCategoryViewModel>> EditAsync(RequestNewsCategoryViewModel responseNewsCategoryViewModel);
        Task<BaseDataResponse<PagedList<NewsCategoryLogViewModel>>> GetAllLogAsync(Guid id, PagingOptions pagingOptions, SortRule sortRule);
        Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetCategoriesWithNewsAllSelectListItemAsync();
    }
}
