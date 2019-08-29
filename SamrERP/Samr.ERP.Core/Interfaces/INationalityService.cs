using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Handbook.Nationality;

namespace Samr.ERP.Core.Interfaces
{
    public interface INationalityService
    {
        Task<BaseDataResponse<PagedList<ResponseNationalityViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule);
        Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllListItemAsync();
        Task<BaseDataResponse<ResponseNationalityViewModel>> GetByIdAsync(Guid id);
        Task<BaseDataResponse<ResponseNationalityViewModel>> CreateAsync(RequestNationalityViewModel nationalityViewModel);
        Task<BaseDataResponse<ResponseNationalityViewModel>> EditAsync(RequestNationalityViewModel nationalityViewModel);
        Task<BaseDataResponse<PagedList<NationalityLogViewModel>>> GetAllLogAsync(Guid id, PagingOptions pagingOptions, SortRule sortRule);
    }
}
