using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Samr.ERP.Core.Services;
using Samr.ERP.WebApi.Hub;
using Microsoft.AspNetCore.SignalR;
using Samr.ERP.Core.ViewModels.Message;
using Samr.ERP.Infrastructure.Entities;


namespace Samr.ERP.WebApi.HubEvent
{
    public class HubEvent
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        private readonly HubConnectionHandler<NotificationHub> _test;


        public HubEvent(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
            MessageService.NotifyNewMessage += OnNewMessage;
            MessageService.NotifyCountChange += OnNotificationCountChange;
            MessageService.NotifyMessageRead += OnMessageRead;
        }


        public  async Task OnNewMessage(GetReceiverMessageViewModel receiverMessageViewModel, GetSenderMessageViewModel senderMessageViewModel )
        {
            await _hubContext.Clients.Group(receiverMessageViewModel.ReceiverUserId.ToString()).SendAsync("OnMessageReceived", receiverMessageViewModel);
            await _hubContext.Clients.Group(senderMessageViewModel.SenderUserId.ToString()).SendAsync("OnMessageSent", senderMessageViewModel);
        }

        public async Task OnNotificationCountChange(int unReadedCount, string userId)
        {
            await _hubContext.Clients.Group(userId).SendAsync("OnNotificationCountChange", unReadedCount, userId);
        }
        public async Task OnMessageRead(Notification notification)
        {
            await _hubContext.Clients.Group(notification.ReceiverUserId.Value.ToString()).SendAsync("OnMessageRead", notification);
            await _hubContext.Clients.Group(notification.SenderUserId.Value.ToString()).SendAsync("OnMessageRead", notification);
        }

        public async Task SendMessage(object sender, string userId)
        {
            //string name = Context.User.Identity.Name;
            //Debug.WriteLine("i am here");
            //await _hubContext.Clients.All.SendAsync("MessageReceived", sender, userId, name);
            //await _hubContext.Clients.User(name).SendAsync("MessageReceived", sender, userId);
        }
    }
}
