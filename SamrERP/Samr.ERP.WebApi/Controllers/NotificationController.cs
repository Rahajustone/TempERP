using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.Notification;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ApiController
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService, HubEvent.HubEvent hubEvent)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<BaseDataResponse<IEnumerable<NotificationSystemViewModel>>> GetSentMessage()
        {
            var response = await _notificationService.GetSentAsync();
            return Response(response);
        }

        [HttpGet]
        public async Task<BaseDataResponse<IEnumerable<NotificationSystemViewModel>>> GetReceivedMessage()
        {
            var response = await _notificationService.GetReceivedAsync();
            return Response(response);
        }

        [HttpPost]
        public async Task<BaseDataResponse<NotificationSystemViewModel>> Create([FromBody]NotificationSystemViewModel model)
        {
            var response = await _notificationService.CreateAsync(model);
            return Response(response);
        }

        [HttpPost]
        public async Task<BaseDataResponse<CreateMessageViewModel>> SentMessage([FromBody]CreateMessageViewModel createMessageViewModel)
        {
            var response = await _notificationService.CreateMessageAsync(createMessageViewModel);
            return Response(response);
        }

        [HttpPost]
        public async Task<BaseResponse> MarkThemAsRead([FromBody] Guid id)
        {
            var response = await _notificationService.MarkThemAsReadAsync(id);

            return Response(response);
        }
    }
}
