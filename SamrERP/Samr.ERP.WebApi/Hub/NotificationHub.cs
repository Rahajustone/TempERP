using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Services;
using Samr.ERP.Core.Staff;
using Samr.ERP.Infrastructure.Providers;
using Samr.ERP.WebApi.Infrastructure;

namespace Samr.ERP.WebApi.Hub
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly IMessageService _messageService;
        private readonly UserProvider _userProvider;
        private readonly IHttpContextAccessor _accessor;


        public NotificationHub(IMessageService messageService)
        {
            _messageService = messageService;
        }
        public async Task MessageReceived()
        {
            string name = Context.User.Identity.Name;

            await Clients.All.SendAsync("test", name);
        }
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User.GetIdClaimValue();
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            await _messageService.NotifyUnreadedMessageCount(Guid.Parse(userId));
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.User.GetIdClaimValue());

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
