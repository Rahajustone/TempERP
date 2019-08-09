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
using Samr.ERP.Infrastructure.Providers;

namespace Samr.ERP.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserProvider _userProvider;

        public static event OnNotificationAdd NotifyMessage;
        public delegate Task OnNotificationAdd(
            object sender, EventArgs e);
        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, UserProvider userProvider)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userProvider = userProvider;
        }
        private IQueryable<Notification> GetQuery()
        {
            return _unitOfWork.Notifications.GetDbSet()
                .OrderByDescending( p => p.CreatedAt);
        }

        public async Task<BaseDataResponse<NotificationSystemViewModel>> CreateAsync(NotificationSystemViewModel notificationSystemViewModel)
        {
            var notification = _mapper.Map<Notification>(notificationSystemViewModel);
            _unitOfWork.Notifications.Add(notification);

            await _unitOfWork.CommitAsync();

            NotifyMessage?.Invoke(this, EventArgs.Empty);

            return BaseDataResponse<NotificationSystemViewModel>.Success(_mapper.Map<NotificationSystemViewModel>(notification));
        }

        public async Task<BaseDataResponse<IEnumerable<NotificationSystemViewModel>>> GetSentAsync()
        {
            var getSentMessage = await GetQuery()
                .Where(m => m.FromUserId == _userProvider.CurrentUser.Id)
                .ToListAsync();

            var vm = _mapper.Map<IEnumerable<NotificationSystemViewModel>>(getSentMessage);

            return BaseDataResponse<IEnumerable<NotificationSystemViewModel>>.Success(vm);
        }

        public async Task<BaseDataResponse<IEnumerable<NotificationSystemViewModel>>> GetReceivedAsync()
        {
            var getReceivedMessage = await GetQuery()
                .Where(m => m.FromUserId != _userProvider.CurrentUser.Id)
                .ToListAsync();

            var vm = _mapper.Map<IEnumerable<NotificationSystemViewModel>>(getReceivedMessage);

            return BaseDataResponse<IEnumerable<NotificationSystemViewModel>>.Success(vm);
        }

        public async Task<BaseDataResponse<CreateMessageViewModel>> CreateMessageAsync(CreateMessageViewModel createMessageViewModel)
        {
            createMessageViewModel.FromUserId = _userProvider.CurrentUser.Id;

            var message = _mapper.Map<Notification>(createMessageViewModel);
            _unitOfWork.Notifications.Add(message);

            await _unitOfWork.CommitAsync();

            //NotifyMessage?.Invoke(this, EventArgs.Empty);

            return BaseDataResponse<CreateMessageViewModel>.Success(_mapper.Map<CreateMessageViewModel>(message));
        }

        public async Task<BaseResponse> MarkThemAsReadAsync(Guid id)
        {
            var message = await _unitOfWork.Notifications.GetDbSet().FirstOrDefaultAsync(m => m.Id == id);

            if (message == null)
            {
                return  BaseResponse.Fail(null);
            }

            message.IsViewed = true;

            await _unitOfWork.CommitAsync();

            return BaseResponse.Success();
        }
    }
}
