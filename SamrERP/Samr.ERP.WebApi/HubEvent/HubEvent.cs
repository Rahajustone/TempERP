using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Samr.ERP.Core.Services;
using Samr.ERP.WebApi.Hub;
using Microsoft.AspNetCore.SignalR;
using Samr.ERP.Core.ViewModels.Message;


namespace Samr.ERP.WebApi.HubEvent
{
    public class HubEvent
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        private readonly HubConnectionHandler<NotificationHub> _test;


        public HubEvent(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
            MessageService.NotifyMessage += OnNotify;
        }


        public  async Task OnNotify(NotifyMessageViewModel notifyMessage, string userId)
        {
            await _hubContext.Clients.All.SendAsync("MessageReceived", notifyMessage, userId);
            //await _hubContext.Clients.Groups(userId).SendAsync("MessageReceived", notifyMessage, userId);

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
