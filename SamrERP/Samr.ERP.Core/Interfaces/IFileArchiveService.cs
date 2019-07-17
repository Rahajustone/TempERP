using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.FileArchive;

namespace Samr.ERP.Core.Interfaces
{
    public interface IFileArchiveService
    {
        Task<BaseDataResponse<FileArchiveViewModel>> GetByIdAsync(Guid id);
        Task<BaseDataResponse<PagedList<FileArchiveViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterFileArchiveViewModel filterFileArchiveViewModel, SortRule sortRule);
        Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllSelectListItemAsync();
        Task<BaseDataResponse<EditFileArchiveViewModel>> CreateAsync(EditFileArchiveViewModel editFileArchiveViewModel);
    }
}
