using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Samr.ERP.Core.Services;
using Samr.ERP.Infrastructure.Providers;

namespace Samr.ERP.WebApi.Hub
{
    public class NotificationHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly UserProvider _user;

        public NotificationHub(UserProvider user)
        {
            _user = user;
        }
        public async Task MessageReceived(object user, object message)
        {
            await Clients.All.SendAsync("MessageReceived", user, message);
            //await Clients.User(_user.CurrentUser.Id.ToString()).SendAsync("SendMessage", user, message);
        }
    }
}
