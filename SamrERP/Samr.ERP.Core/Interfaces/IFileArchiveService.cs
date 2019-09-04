using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Staff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.FileArchive;

namespace Samr.ERP.Core.Interfaces
{
    public interface IFileArchiveService
    {
        Task<BaseDataResponse<GetByIdFileArchiveViewModel>> GetByIdAsync(Guid id);
        Task<BaseDataResponse<PagedList<GetListFileArchiveViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterFileArchiveViewModel filterFileArchiveViewModel, SortRule sortRule);
        Task<BaseDataResponse<GetByIdFileArchiveViewModel>> CreateAsync(CreateFileArchiveViewModel createFileArchive);
        Task<BaseDataResponse<GetByIdFileArchiveViewModel>> EditAsync(EditFileArchiveViewModel editFileArchiveViewModel);
    }
}
