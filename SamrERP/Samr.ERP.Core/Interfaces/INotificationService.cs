using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.Notification;

namespace Samr.ERP.Core.Interfaces
{
    public interface INotificationService
    {
        Task<BaseDataResponse<NotificationSystemViewModel>> CreateAsync(NotificationSystemViewModel notificationSystemViewModel);
    }
}
