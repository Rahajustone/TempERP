using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Staff;
using Samr.ERP.Core.ViewModels.Message;

namespace Samr.ERP.Core.Interfaces
{
    public interface IMessageService
    {
        Task<BaseDataResponse<GetSenderMessageViewModel>> SendMessageAsync(CreateMessageViewModel notificationSystemViewModel);
        Task<BaseDataResponse<PagedList<ReceiverMessageViewModel>>> GetReceivedMessagesAsync(PagingOptions pagingOptions, FilterMessageViewModel fileFilterMessageViewModel);
        Task<BaseDataResponse<PagedList<SenderMessageViewModel>>> GetSentMessagesAsync(PagingOptions pagingOptions, FilterMessageViewModel fileFilterMessageViewModel);
        Task<BaseDataResponse<GetReceiverMessageViewModel>> GetReceivedMessageAsync(Guid id);
        Task<BaseDataResponse<GetSenderMessageViewModel>> GetSentMessageAsync(Guid id);
        Task NotifyUnreadedMessageCount(Guid userId);
    }
}
