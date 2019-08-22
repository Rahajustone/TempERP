using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Enums;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.EmailSetting;
using Samr.ERP.Core.ViewModels.SMPPSetting;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class SMPPSettingService : ISMPPSettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SMPPSettingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public SMPPSetting GetDefaultSMPPSetting()
        {
            return _unitOfWork.SMPPSettings.All().OrderByDescending(p => p.IsDefault).FirstOrDefault(p => p.IsActive);
        }

        private async Task UndefaultOthersAsync(Guid id)
        {
            var smppSettings = await _unitOfWork.SMPPSettings.All().Where(p => p.IsActive && p.IsDefault && p.Id != id).ToListAsync();
            foreach (var setting in smppSettings)
            {
                setting.IsDefault = false;

                _unitOfWork.SMPPSettings.Update(setting);
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task<BaseDataResponse<SMPPSettingResponseViewModel>> CreateAsync(SMPPSettingViewModel smppSettingViewModel)
        {
            var smppSettingExist = _unitOfWork.SMPPSettings.Any(p => p.Host == smppSettingViewModel.Host && p.SystemId == smppSettingViewModel.SystemId);
            
            if (smppSettingExist) return BaseDataResponse<SMPPSettingResponseViewModel>.Fail( _mapper.Map<SMPPSettingResponseViewModel>(smppSettingViewModel), new ErrorModel(ErrorCode.SystemIdAndHostMustBeUnique));

            var smppSetting = _mapper.Map<SMPPSetting>(smppSettingViewModel);
            
            _unitOfWork.SMPPSettings.Add(smppSetting);

            await _unitOfWork.CommitAsync();

            if (smppSetting.IsDefault) await UndefaultOthersAsync(smppSetting.Id);

            return BaseDataResponse<SMPPSettingResponseViewModel>.Success(_mapper.Map<SMPPSettingResponseViewModel>(smppSetting));
        }

        public async Task<BaseDataResponse<SMPPSettingResponseViewModel>> EditAsync(SMPPSettingViewModel smppSettingViewModel)
        {
            var smppSettingExist = _unitOfWork.SMPPSettings.Any(p => p.Id == smppSettingViewModel.Id);
            if (!smppSettingExist)
            {
                return BaseDataResponse<SMPPSettingResponseViewModel>.NotFound(null);
            }

            var smppSettingUnique = _unitOfWork.SMPPSettings.Any(p =>
                p.Id != smppSettingViewModel.Id && p.Host == smppSettingViewModel.Host &&
                p.SystemId == smppSettingViewModel.SystemId);

            if (smppSettingUnique) return BaseDataResponse<SMPPSettingResponseViewModel>.Fail(_mapper.Map<SMPPSettingResponseViewModel>(smppSettingViewModel), new ErrorModel(ErrorCode.SystemIdAndHostMustBeUnique));

            var smppSetting = _mapper.Map<SMPPSetting>(smppSettingViewModel);

            _unitOfWork.SMPPSettings.Update(smppSetting);

            await _unitOfWork.CommitAsync();

            if (smppSetting.IsDefault) await UndefaultOthersAsync(smppSetting.Id);

            return BaseDataResponse<SMPPSettingResponseViewModel>.Success(
                    _mapper.Map<SMPPSettingResponseViewModel>(smppSetting));
        }

        public async Task<BaseDataResponse<SMPPSettingResponseViewModel>> GetByIdAsync(Guid id)
        {
            var smppSetting = await _unitOfWork.SMPPSettings.GetByIdAsync(id);

            return smppSetting != null
                ? BaseDataResponse<SMPPSettingResponseViewModel>.Success(_mapper.Map<SMPPSettingResponseViewModel>(smppSetting))
                : BaseDataResponse<SMPPSettingResponseViewModel>.NotFound(null);
        }

        public BaseDataResponse<IEnumerable<SMPPSettingResponseViewModel>> GetAllAsync()
        {
            return BaseDataResponse<IEnumerable<SMPPSettingResponseViewModel>>
                .Success(_unitOfWork.SMPPSettings.GetAll().Select(p => _mapper.Map<SMPPSettingResponseViewModel>(p)));
        }

        public async Task<BaseResponse> Delete(Guid id)
        {
            var smppSetting = await _unitOfWork.SMPPSettings.GetByIdAsync(id);
            if (smppSetting != null)
            {
                _unitOfWork.SMPPSettings.Delete(id);

                await _unitOfWork.CommitAsync();

                return BaseResponse.Success();
            }

            return BaseResponse.NotFound();
        }
    }
}
