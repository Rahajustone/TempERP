using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Samr.ERP.Core.Services;
using Samr.ERP.Infrastructure.Migrations;
using Samr.ERP.Infrastructure.Providers;
using Samr.ERP.WebApi.Hub;

namespace Samr.ERP.WebApi.HubEvent
{
    public class HubEvent
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public HubEvent(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
            MessageService.NotifyMessage += OnNotify;
        }


        //[Authorize]
        public  async Task OnNotify(object sender, EventArgs args, string userId)
        {
            Debug.WriteLine("i am here");
            await _hubContext.Clients.All.SendAsync("MessageReceived", sender, userId);
            await _hubContext.Clients.User(userId).SendAsync("MessageReceived", sender, userId);
        }
    }
}
