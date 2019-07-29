using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Samr.ERP.Core.Services;

namespace Samr.ERP.WebApi.Hub
{
    public class NotificationHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public NotificationHub()
        {
            NotificationService.NotifyMessage += OnNotify;
        }

        private async Task OnNotify(object sender,EventArgs args)
        {
            await SendMessage("test", "test");
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("SendMessage", user, message);
        }
    }
}
