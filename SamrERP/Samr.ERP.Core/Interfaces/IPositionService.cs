using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Position;

namespace Samr.ERP.Core.Interfaces
{
    public interface IPositionService
    {
        Task<BaseDataResponse<EditPositionViewModel>> GetByIdAsync(Guid id);
        Task<BaseDataResponse<PagedList<PositionViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule);
        Task<BaseDataResponse<IEnumerable<PositionViewModel>>> GetAllByDepartmentId(Guid id);
        Task<BaseDataResponse<EditPositionViewModel>> CreateAsync(EditPositionViewModel positionViewModel);
        Task<BaseDataResponse<EditPositionViewModel>> EditAsync(EditPositionViewModel positionViewModel);
    }
}
