using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Handbook.UsefulLinkCategory;
using Samr.ERP.Core.ViewModels.UsefulLink;
using Samr.ERP.Core.ViewModels.UsefulLink.UsefulLinkCategory;

namespace Samr.ERP.Core.Interfaces
{
    public interface IUsefulLinkService
    {
        Task<BaseDataResponse<UsefulLinkViewModel>> GetByIdAsync(Guid id);
        Task<BaseDataResponse<PagedList<UsefulLinkViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterUsefulLinkViewModel filterUsefulLinkViewModel);
        Task<BaseDataResponse<UsefulLinkViewModel>> CreateAsync(UsefulLinkViewModel model);
        Task<BaseDataResponse<UsefulLinkViewModel>> EditAsync(UsefulLinkViewModel model);
        Task<BaseDataResponse<PagedList<UsefulLinkCategoryLogViewModel>>> GetAllLogAsync(Guid id, PagingOptions pagingOptions, SortRule sortRule);
    }
}