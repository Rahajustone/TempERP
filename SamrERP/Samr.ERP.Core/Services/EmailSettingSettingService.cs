using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class EmailSettingSettingService:IEmailSettingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmailSettingSettingService(
            IUnitOfWork unitOfWork
            )
        {
            _unitOfWork = unitOfWork;
        }

        public EmailSetting GetDefaultEmailSetting()
        {
            return _unitOfWork.EmailSettings.All().FirstOrDefault(p => p.IsActive && p.IsDefault);
        }
    }

   
}
