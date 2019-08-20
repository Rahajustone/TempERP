using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.SMPPSetting;

namespace Samr.ERP.Core.Interfaces
{
    public interface ISMPPSettingService
    {
        Task<BaseDataResponse<SMPPSettingResponseViewModel>> CreateAsync(SMPPSettingViewModel smppSettingViewModel);
        Task<BaseDataResponse<SMPPSettingResponseViewModel>> EditAsync(SMPPSettingViewModel smppSettingViewModel);
        Task<BaseDataResponse<SMPPSettingResponseViewModel>> GetByIdAsync(Guid id);
        BaseDataResponse<IEnumerable<SMPPSettingResponseViewModel>> GetAllAsync();
        Task<BaseResponse> Delete(Guid id);
    }
}
