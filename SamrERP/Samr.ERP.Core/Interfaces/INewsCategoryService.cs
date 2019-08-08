using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Handbook.NewCategories;

namespace Samr.ERP.Core.Interfaces
{
    public interface INewsCategoryService
    {
        Task<BaseDataResponse<NewsCategoriesViewModel>> GetByIdAsync(Guid id);
        Task<BaseDataResponse<PagedList<NewsCategoriesViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule);
        Task<BaseDataResponse<NewsCategoriesViewModel>> CreateAsync(NewsCategoriesViewModel newsCategoriesViewModel);
        Task<BaseDataResponse<NewsCategoriesViewModel>> EditAsync(NewsCategoriesViewModel newsCategoriesViewModel);
        Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllSelectListItemAsync();
    }
}
