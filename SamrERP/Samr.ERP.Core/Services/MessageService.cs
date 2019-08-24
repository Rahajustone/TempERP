using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Employee;
using Samr.ERP.Core.ViewModels.Message;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.Infrastructure.Providers;

namespace Samr.ERP.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserProvider _userProvider;

        public static event OnNotificationReceive NotifyMessage;
        public delegate Task OnNotificationReceive(GetReceiverMessageViewModel receivedMessageVm, GetSenderMessageViewModel senderMessageVm);

        public static event OnNotificationCountChange NotifyCountChange;
        public delegate Task OnNotificationCountChange(int unReadedCount, string userId);

        public static event OnMessageRead NotifyMessageRead;
        public delegate Task OnMessageRead(Notification notification);


        public MessageService(IUnitOfWork unitOfWork, IMapper mapper, UserProvider userProvider)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userProvider = userProvider;
        }


        private IQueryable<Notification> GetQuery()
        {
            return _unitOfWork.Notifications.GetDbSet()
                .Include(u => u.CreatedUser)
                .OrderByDescending(p => p.CreatedAt);
        }

        private IQueryable<Notification> FilterQuery(FilterMessageViewModel fileFilterMessageViewModel, IQueryable<Notification> query)
        {
            if (fileFilterMessageViewModel.FromDate != null)
            {
                var fromDate = Convert.ToDateTime(fileFilterMessageViewModel.FromDate);
                query = query.Where(p => p.CreatedAt.Date >= fromDate);
            }

            if (fileFilterMessageViewModel.ToDate != null)
            {
                var toDate = Convert.ToDateTime(fileFilterMessageViewModel.ToDate);
                query = query.Where(p => p.CreatedAt.Date <= toDate);
            }

            return query;
        }

        public async Task<BaseDataResponse<GetSenderMessageViewModel>> SendMessageAsync(CreateMessageViewModel notificationSystemViewModel)
        {
            var notification = _mapper.Map<Notification>(notificationSystemViewModel);
            notification.SenderUserId = _userProvider.CurrentUser.Id;

            _unitOfWork.Notifications.Add(notification);

            await _unitOfWork.CommitAsync();

            var senderMessageViewModel = await GetSentMessageAsync(notification.Id);

            var messageExists = await GetQuery()
                .Include(p => p.SenderUser)
                .ThenInclude(p => p.Employee)
                .ThenInclude(p => p.Position)
                .FirstOrDefaultAsync(p => p.Id == notification.Id);

            var receivedMessageVm = _mapper.Map<GetReceiverMessageViewModel>(messageExists);

            if (receivedMessageVm.User.PhotoPath != null)
            {
                receivedMessageVm.User.PhotoPath = FileService.GetDownloadAction(FileService.GetResizedPath(receivedMessageVm.User.PhotoPath));
            }

            NotifyMessage?.Invoke(receivedMessageVm, senderMessageViewModel.Data);
            await NotifyUnreadedMessageCount(notification.ReceiverUserId.Value);

            return senderMessageViewModel;
        }

        public async Task NotifyUnreadedMessageCount(Guid userId)
        {
            var unReadedCount = await _unitOfWork.Notifications.GetDbSet().CountAsync(p => p.ReceiverUserId == userId && !p.ReadDate.HasValue);

            NotifyCountChange?.Invoke(unReadedCount, userId.ToString());

        }
        public async Task<BaseDataResponse<PagedList<ReceiverMessageViewModel>>> GetReceivedMessagesAsync(PagingOptions pagingOptions, FilterMessageViewModel fileFilterMessageViewModel)
        {
            var query = GetQuery()
                .Include(p => p.SenderUser)
                .Where(m => m.ReceiverUserId == _userProvider.CurrentUser.Id);

            query = FilterQuery(fileFilterMessageViewModel, query);

            if (fileFilterMessageViewModel.Title != null)
            {
                var titleFilter = fileFilterMessageViewModel.Title.ToLower();

                query = query.Where(f => EF.Functions.Like(
                    string.Concat(f.Title.ToLower(), Extension.FullNameToString(f.SenderUser.Employee.LastName, f.SenderUser.Employee.FirstName, f.SenderUser.Employee.MiddleName))
                    , "%" + titleFilter + "%"));
            }
            var queryVm = query.ProjectTo<ReceiverMessageViewModel>();

            var pagedList = await queryVm.ToPagedListAsync(pagingOptions);

            foreach (var allUser in pagedList.Items)
            {
                allUser.User.PhotoPath =
                    FileService.GetDownloadAction(FileService.GetResizedPath(allUser.User.PhotoPath));
            }

            return BaseDataResponse<PagedList<ReceiverMessageViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<PagedList<SenderMessageViewModel>>> GetSentMessagesAsync(PagingOptions pagingOptions, FilterMessageViewModel fileFilterMessageViewModel)
        {
            var query = GetQuery()
                .Include(p => p.ReceiverUser)
                .Where(m => m.SenderUserId == _userProvider.CurrentUser.Id);

            query = FilterQuery(fileFilterMessageViewModel, query);

            if (fileFilterMessageViewModel.Title != null)
            {
                var titleFilter = fileFilterMessageViewModel.Title.ToLower();

                query = query.Where(f => EF.Functions.Like(
                    string.Concat(f.Title.ToLower(), Extension.FullNameToString(f.ReceiverUser.Employee.LastName, f.ReceiverUser.Employee.FirstName, f.ReceiverUser.Employee.MiddleName))
                    , "%" + titleFilter + "%"));
            }

            var queryVm = query.ProjectTo<SenderMessageViewModel>();

            var pagedList = await queryVm.ToPagedListAsync(pagingOptions);

            foreach (var allUser in pagedList.Items)
            {
                allUser.User.PhotoPath =
                    FileService.GetDownloadAction(FileService.GetResizedPath(allUser.User.PhotoPath));
            }

            return BaseDataResponse<PagedList<SenderMessageViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<GetReceiverMessageViewModel>> GetReceivedMessageAsync(Guid id)
        {
            var messageExists = await GetQuery()
                .Include(p => p.SenderUser)
                .ThenInclude(p => p.Employee)
                .ThenInclude(p => p.Position)
                .Where(p => p.ReceiverUserId == _userProvider.CurrentUser.Id)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (messageExists != null)
            {
                if (!messageExists.ReadDate.HasValue)
                {
                    messageExists.ReadDate = DateTime.Now;
                    await _unitOfWork.CommitAsync();
                    await NotifyUnreadedMessageCount(messageExists.ReceiverUserId.Value);
                    // ReSharper disable once PossibleNullReferenceException
                    await NotifyMessageRead?.Invoke(messageExists);

                }

                var vm = _mapper.Map<GetReceiverMessageViewModel>(messageExists);

                if (vm.User.PhotoPath != null)
                {
                    vm.User.PhotoPath = FileService.GetDownloadAction(FileService.GetResizedPath(vm.User.PhotoPath));
                }

                return BaseDataResponse<GetReceiverMessageViewModel>.Success(vm);
            }

            return BaseDataResponse<GetReceiverMessageViewModel>.NotFound(null);
        }

        public async Task<BaseDataResponse<GetSenderMessageViewModel>> GetSentMessageAsync(Guid id)
        {
            var vm = await GetSentMessage(id);

            return BaseDataResponse<GetSenderMessageViewModel>.Success(vm);
        }

        private async Task<GetSenderMessageViewModel> GetSentMessage(Guid id)
        {
            var messageExists = await GetQuery()
                .Include(p => p.ReceiverUser)
                .ThenInclude(p => p.Employee)
                .ThenInclude(p => p.Position)
                .Where(p => p.CreatedUserId == _userProvider.CurrentUser.Id)
                .FirstOrDefaultAsync(p => p.Id == id);

            var vm = _mapper.Map<GetSenderMessageViewModel>(messageExists);
            if (vm.User.PhotoPath != null)
            {
                vm.User.PhotoPath = FileService.GetDownloadAction(FileService.GetResizedPath(vm.User.PhotoPath));
            }

            return vm;
        }
    }
}
