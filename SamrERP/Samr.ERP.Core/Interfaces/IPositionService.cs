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
        Task<BaseResponse<EditPositionViewModel>> GetByIdAsync(Guid id);
        Task<BaseResponse<IEnumerable<PositionViewModel>>> GetAllAsync();
        Task<BaseResponse<EditPositionViewModel>> CreateAsync(EditPositionViewModel positionViewModel);
        Task<BaseResponse<EditPositionViewModel>> UpdateAsync(EditPositionViewModel positionViewModel);
    }
}
