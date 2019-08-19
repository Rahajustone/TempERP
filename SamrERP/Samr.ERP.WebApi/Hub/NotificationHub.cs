using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.SignalR;
using Samr.ERP.Core.Services;
using Samr.ERP.Infrastructure.Providers;
using Samr.ERP.WebApi.Infrastructure;

namespace Samr.ERP.WebApi.Hub
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly UserProvider _userProvider;

        
        public NotificationHub(UserProvider userProvider)
        {
            _userProvider = userProvider;
        }
        public async Task MessageReceived()
        {
            string name = Context.User.Identity.Name;

            await Clients.All.SendAsync("test", name);
        }
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, _userProvider.CurrentUser.Id.ToString());

            await base.OnConnectedAsync();
        }

        public override  Task OnDisconnectedAsync(Exception exception)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, _userProvider.CurrentUser.ToString());

            return base.OnDisconnectedAsync(exception);
        }

        //public override Task OnReconnected()
        //{
        //    Groups.AddToGroupAsync().

        //    if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
        //    {
        //        _connections.Add(name, Context.ConnectionId);
        //    }

        //    return base.OnReconnected();
        //}
    }
}
