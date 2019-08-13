using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Notification;

namespace Samr.ERP.Core.Interfaces
{
    public interface INotificationService
    {
        Task<BaseDataResponse<NotificationSystemViewModel>> SendMessageAsync(CreateMessageViewModel notificationSystemViewModel);
        Task<BaseDataResponse<PagedList<NotificationSystemViewModel>>> GetReceivedMessagesAsync(PagingOptions pagingOptions, FilterNotificationViewModel fileFilterNotificationViewModel);
        Task<BaseDataResponse<PagedList<NotificationSystemViewModel>>> GetSentMessagesAsync(PagingOptions pagingOptions, FilterNotificationViewModel fileFilterNotificationViewModel);
        Task<BaseDataResponse<NotificationSystemViewModel>> GetReceivedMessageAsync(Guid id);
        Task<BaseDataResponse<NotificationSystemViewModel>> GetSentMessageAsync(Guid id);
    }
}
