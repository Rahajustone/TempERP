using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Staff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Position;

namespace Samr.ERP.Core.Interfaces
{
    public interface IPositionService
    {
        Task<BaseDataResponse<ResponsePositionViewModel>> GetByIdAsync(Guid id);
        Task<BaseDataResponse<PagedList<ResponsePositionViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterPositionViewModel filterPosition, SortRule sortRule);
        Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllByDepartmentId(Guid id);
        Task<BaseDataResponse<ResponsePositionViewModel>> CreateAsync(RequestPositionViewModel positionViewModel);
        Task<BaseDataResponse<ResponsePositionViewModel>> EditAsync(RequestPositionViewModel positionViewModel);
        Task<BaseDataResponse<PagedList<PositionLogViewModel>>> GetAllLogAsync(Guid id, PagingOptions pagingOptions, SortRule sortRule);
    }
}
