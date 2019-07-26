using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Handbook.FileCategory;

namespace Samr.ERP.Core.Interfaces
{
    public interface IFileCategoryService
    {
        Task<BaseDataResponse<EditFileCategoryViewModel>> GetByIdAsync(Guid id);
        Task<BaseDataResponse<PagedList<EditFileCategoryViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule);
        Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllSelectListItemAsync();
        Task<BaseDataResponse<EditFileCategoryViewModel>> CreateAsync(EditFileCategoryViewModel fileCategoryViewModel);
        Task<BaseDataResponse<EditFileCategoryViewModel>> EditAsync(EditFileCategoryViewModel editFileCategoryViewModel);
    }
}
