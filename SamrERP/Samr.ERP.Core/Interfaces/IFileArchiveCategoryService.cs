using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Staff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Handbook.FileArchiveCategory;

namespace Samr.ERP.Core.Interfaces
{
    public interface IFileArchiveCategoryService
    {
        Task<BaseDataResponse<ResponseFileArchiveCategoryViewModel>> GetByIdAsync(Guid id);
        Task<BaseDataResponse<PagedList<ResponseFileArchiveCategoryViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule);
        Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllSelectListItemAsync();
        Task<BaseDataResponse<ResponseFileArchiveCategoryViewModel>> CreateAsync(RequestFileArchiveCategoryViewModel fileArchiveCategoryViewModel);
        Task<BaseDataResponse<ResponseFileArchiveCategoryViewModel>> EditAsync(RequestFileArchiveCategoryViewModel editFileArchiveCategoryViewModel);
        Task<BaseDataResponse<PagedList<FileArchiveCategoryLogViewModel>>> GetAllLogAsync(Guid id, PagingOptions pagingOptions, SortRule sortRule);
        Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetCategoriesWithFileArchiveAllSelectListItemAsync();
    }
}
