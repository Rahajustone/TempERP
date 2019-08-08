using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.UsefulLink.UsefulLinkCategory;

namespace Samr.ERP.Core.Interfaces
{
    public interface IUsefulLinkCategoryService
    {
        Task<BaseDataResponse<EditUsefulLinkCategoryViewModel>> GetByIdAsync(Guid id);
        Task<BaseDataResponse<PagedList<EditUsefulLinkCategoryViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule);
        Task<BaseDataResponse<EditUsefulLinkCategoryViewModel>> CreateAsync(EditUsefulLinkCategoryViewModel editUsefulLinkCategoryViewModel);
        Task<BaseDataResponse<EditUsefulLinkCategoryViewModel>> EditAsync(EditUsefulLinkCategoryViewModel editUsefulLinkCategoryViewModel);
        Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllSelectListItemAsync();
    }
}
