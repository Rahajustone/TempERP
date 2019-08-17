using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.SignalR;
using Samr.ERP.Core.Services;
using Samr.ERP.Infrastructure.Providers;

namespace Samr.ERP.WebApi.Hub
{
    public class NotificationHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public NotificationHub()
        {
        }
        public async Task MessageReceived(object user, object message)
        {
            /*Context*/

            await Clients.All.SendAsync("MessageReceived", user, message);
            //await Clients.User(_user.CurrentUser.Id.ToString()).SendAsync("SendMessage", user, message);
        }
    }
}
