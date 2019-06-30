using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.Position;

namespace Samr.ERP.Core.Interfaces
{
    public interface IPositionService
    {
        Task<BaseDataResponse<EditPositionViewModel>> GetByIdAsync(Guid id);
        Task<BaseDataResponse<IEnumerable<PositionViewModel>>> GetAllAsync();
        Task<BaseDataResponse<EditPositionViewModel>> CreateAsync(EditPositionViewModel positionViewModel);
        Task<BaseDataResponse<EditPositionViewModel>> UpdateAsync(EditPositionViewModel positionViewModel);
    }
}
