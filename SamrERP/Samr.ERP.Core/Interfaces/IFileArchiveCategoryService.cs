using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Handbook.FileArchiveCategory;

namespace Samr.ERP.Core.Interfaces
{
    public interface IFileArchiveCategoryService
    {
        Task<BaseDataResponse<EditFileArchiveCategoryViewModel>> GetByIdAsync(Guid id);
        Task<BaseDataResponse<PagedList<EditFileArchiveCategoryViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule);
        Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllSelectListItemAsync();
        Task<BaseDataResponse<EditFileArchiveCategoryViewModel>> CreateAsync(EditFileArchiveCategoryViewModel fileArchiveCategoryViewModel);
        Task<BaseDataResponse<EditFileArchiveCategoryViewModel>> EditAsync(EditFileArchiveCategoryViewModel editFileArchiveCategoryViewModel);
        Task<BaseDataResponse<PagedList<FileArchiveCategoryLogViewModel>>> GetAllLogAsync(Guid id, PagingOptions pagingOptions, SortRule sortRule);
    }
}
