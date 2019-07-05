using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.EmailSetting;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class EmailSettingSettingService : IEmailSettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmailSettingSettingService(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public EmailSetting GetDefaultEmailSetting()
        {
            return _unitOfWork.EmailSettings.All().FirstOrDefault(p => p.IsActive && p.IsDefault);
        }

        public async Task<BaseDataResponse<EmailSettingViewModel>> CreateAsync(EmailSettingViewModel emailSettingView)
        {
            var emailSettingExist = _unitOfWork.EmailSettings.Any(p => p.Sender == emailSettingView.Sender);

            if (emailSettingExist) return BaseDataResponse<EmailSettingViewModel>.Fail(emailSettingView, new ErrorModel("emailsetting exist with same sender"));

            var emailSetting = _mapper.Map<EmailSetting>(emailSettingView);

            if (emailSetting.IsDefault) SetDefaultAsync(emailSetting);

            _unitOfWork.EmailSettings.Add(emailSetting);

            await _unitOfWork.CommitAsync();
            

            return BaseDataResponse<EmailSettingViewModel>.Success(_mapper.Map<EmailSettingViewModel>(emailSetting));
        }

        public async Task<BaseDataResponse<EmailSettingViewModel>> UpdateAsync(
            EmailSettingViewModel emailSettingViewModel)
        {
            var emailSettingExist =  await _unitOfWork.EmailSettings.AnyAsync(p => p.Sender == emailSettingViewModel.Sender && p.IsActive);

            if (emailSettingExist) return BaseDataResponse<EmailSettingViewModel>.Fail(emailSettingViewModel, new ErrorModel("emailsetting exist with same sender"));

            var emailSetting = _mapper.Map<EmailSetting>(emailSettingViewModel);

            _unitOfWork.EmailSettings.Update(emailSetting);

            await _unitOfWork.CommitAsync();

            if (emailSettingViewModel.IsDefault) SetDefaultAsync(emailSetting);

            return BaseDataResponse<EmailSettingViewModel>.Success(_mapper.Map<EmailSettingViewModel>(emailSetting));
            ;
        }

        public async Task<BaseDataResponse<EmailSettingViewModel>> GetByIdAsync(Guid id)
        {
            var emailSetting = await _unitOfWork.EmailSettings.GetByIdAsync(id);

            return emailSetting != null
                ? BaseDataResponse<EmailSettingViewModel>.Success(_mapper.Map<EmailSettingViewModel>(emailSetting))
                : BaseDataResponse<EmailSettingViewModel>.NotFound(null);
        }
        

        private async void SetDefaultAsync(EmailSetting emailSetting)
        {
            var emailSettings = await _unitOfWork.EmailSettings.All().Where(p => p.IsActive && p.IsDefault && p.Id != emailSetting.Id).ToListAsync();
            foreach (var setting in emailSettings)
            {
                setting.IsDefault = false;

                _unitOfWork.EmailSettings.Update(setting);
            }

            emailSetting.IsDefault = true;
            _unitOfWork.EmailSettings.Update(emailSetting);

            await _unitOfWork.CommitAsync();
        }

        public BaseDataResponse<IEnumerable<EmailSettingViewModel>> GetAll()
        {
            return BaseDataResponse<IEnumerable<EmailSettingViewModel>>
                .Success(_unitOfWork.EmailSettings.GetAll().Select(p => _mapper.Map<EmailSettingViewModel>(p)));
        }


    }


}
