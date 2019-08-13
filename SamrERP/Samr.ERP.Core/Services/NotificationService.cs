using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
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
                .Include( u => u.CreatedUser)
                .OrderByDescending( p => p.CreatedAt);
        }

        private IQueryable<Notification> FilterQuery(FilterNotificationViewModel fileFilterNotificationViewModel, IQueryable<Notification> query)
        {
            if (fileFilterNotificationViewModel.FromDate != null)
            {
                var fromDate = Convert.ToDateTime(fileFilterNotificationViewModel.FromDate);
                query = query.Where(p => p.CreatedAt.Date >= fromDate);
            }

            if (fileFilterNotificationViewModel.ToDate != null)
            {
                var toDate = Convert.ToDateTime(fileFilterNotificationViewModel.ToDate);
                query = query.Where(p => p.CreatedAt.Date <= toDate);
            }

            if (fileFilterNotificationViewModel.Title != null)
            {
                var titleFilter = fileFilterNotificationViewModel.Title.ToLower();
                query = query.Where(f => EF.Functions.Like(f.Title.ToLower() + " " + f.CreatedUser.Employee.FullName().ToLower(), "%" + titleFilter + "%"));
            }

            return query;
        }

        public async Task<BaseDataResponse<NotificationSystemViewModel>> SendMessageAsync(CreateMessageViewModel notificationSystemViewModel)
        {
            var notification = _mapper.Map<Notification>(notificationSystemViewModel);
            notification.SenderUserId = _userProvider.CurrentUser.Id;

            _unitOfWork.Notifications.Add(notification);

            await _unitOfWork.CommitAsync();

            NotifyMessage?.Invoke(this, EventArgs.Empty);

            var createdNotification = await GetSentMessageAsync(notification.Id);

            return createdNotification;
        }

        public async Task<BaseDataResponse<PagedList<NotificationSystemViewModel>>> GetReceivedMessagesAsync(PagingOptions pagingOptions, FilterNotificationViewModel fileFilterNotificationViewModel)
        {
            var query = GetQuery().Where(m => m.ReceiverUserId == _userProvider.CurrentUser.Id);

            query = FilterQuery(fileFilterNotificationViewModel, query);
            var queryVm = query.ProjectTo<NotificationSystemViewModel>();
            var pagedList = await queryVm.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<NotificationSystemViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<PagedList<NotificationSystemViewModel>>> GetSentMessagesAsync(PagingOptions pagingOptions, FilterNotificationViewModel fileFilterNotificationViewModel)
        {
            var query = GetQuery().Where(m => m.SenderUserId == _userProvider.CurrentUser.Id);

            query = FilterQuery(fileFilterNotificationViewModel, query);

            var queryVm = query.ProjectTo<NotificationSystemViewModel>();

            var pagedList = await queryVm.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<NotificationSystemViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<NotificationSystemViewModel>> GetReceivedMessageAsync(Guid id)
        {
            var messageExists = await _unitOfWork.Notifications.GetDbSet()
                .Include(p => p.CreatedUser)
                .Where( p => p.ReceiverUserId == _userProvider.CurrentUser.Id)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (messageExists != null)
            {
                if (!(messageExists.ReadDate.HasValue))
                {
                    messageExists.ReadDate = DateTime.Now;
                    await _unitOfWork.CommitAsync();
                }

                return BaseDataResponse<NotificationSystemViewModel>.Success(_mapper.Map<NotificationSystemViewModel>(messageExists));
            }

            return  BaseDataResponse<NotificationSystemViewModel>.NotFound(null);
        }

        public async Task<BaseDataResponse<NotificationSystemViewModel>> GetSentMessageAsync(Guid id)
        {
            var messageExists = await _unitOfWork.Notifications.GetDbSet()
                .Include(p => p.CreatedUser)
                .Where(p => p.SenderUserId == _userProvider.CurrentUser.Id)
                .FirstOrDefaultAsync(p => p.Id == id);
            
            return BaseDataResponse<NotificationSystemViewModel>.Success(_mapper.Map<NotificationSystemViewModel>(messageExists));
        }
    }
}
