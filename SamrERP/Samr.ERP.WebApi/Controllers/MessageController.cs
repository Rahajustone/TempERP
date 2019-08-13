using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Notification;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MessageController : ApiController
    {
        private readonly INotificationService _notificationService;

        public MessageController(INotificationService notificationService, HubEvent.HubEvent hubEvent)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<BaseDataResponse<PagedList<NotificationSystemViewModel>>> GetReceivedMessages([FromQuery]PagingOptions pagingOptions, [FromQuery]FilterNotificationViewModel filterNotificationViewModel)
        {
            var response = await _notificationService.GetReceivedMessagesAsync(pagingOptions, filterNotificationViewModel);
            return Response(response);
        }

        [HttpGet]
        public async Task<BaseDataResponse<PagedList<NotificationSystemViewModel>>> GetSentMessages([FromQuery]PagingOptions pagingOptions, [FromQuery]FilterNotificationViewModel filterNotificationViewModel)
        {
            var response = await _notificationService.GetSentMessagesAsync(pagingOptions, filterNotificationViewModel);
            return Response(response);
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<NotificationSystemViewModel>> GetReceivedMessage(Guid id)
        {
            var response = await _notificationService.GetReceivedMessageAsync(id);
            return Response(response);
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<NotificationSystemViewModel>> GetSentMessage(Guid id)
        {
            var response = await _notificationService.GetSentMessageAsync(id);
            return Response(response);
        }

        [HttpPost]
        public async Task<BaseDataResponse<NotificationSystemViewModel>> Create([FromBody]CreateMessageViewModel messageViewModel)
        {
            var response = await _notificationService.SendMessageAsync(messageViewModel);
            return Response(response);
        }
    }
}
