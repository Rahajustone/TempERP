using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Notification;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public static event OnNotificationAdd NotifyMessage;
        public delegate Task OnNotificationAdd(
            object sender, EventArgs e);
        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        private DbSet<Notification> GetQuery()
        {
            return _unitOfWork.Notifications.GetDbSet();
        }

        public async Task<BaseDataResponse<NotificationSystemViewModel>> CreateAsync(NotificationSystemViewModel notificationSystemViewModel)
        {
            var notification = _mapper.Map<Notification>(notificationSystemViewModel);
            _unitOfWork.Notifications.Add(notification);

            await _unitOfWork.CommitAsync();

            NotifyMessage?.Invoke(this, EventArgs.Empty);

            return BaseDataResponse<NotificationSystemViewModel>.Success(_mapper.Map<NotificationSystemViewModel>(notification));
        }
    }
}
