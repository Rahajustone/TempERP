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
using Samr.ERP.Core.Staff;
using Samr.ERP.Core.ViewModels.Message;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MessageController : ApiController
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService, HubEvent.HubEvent hubEvent)
        {
            _messageService = messageService;
        }

        [HttpGet]
        public async Task<BaseDataResponse<PagedList<ReceiverMessageViewModel>>> GetReceivedMessages([FromQuery]PagingOptions pagingOptions, [FromQuery]FilterMessageViewModel filterMessageViewModel)
        {
            var response = await _messageService.GetReceivedMessagesAsync(pagingOptions, filterMessageViewModel);
            return Response(response);
        }

        [HttpGet]
        public async Task<BaseDataResponse<PagedList<SenderMessageViewModel>>> GetSentMessages([FromQuery]PagingOptions pagingOptions, [FromQuery]FilterMessageViewModel filterMessageViewModel)
        {
            var response = await _messageService.GetSentMessagesAsync(pagingOptions, filterMessageViewModel);
            return Response(response);
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<GetReceiverMessageViewModel>> GetReceivedMessage(Guid id)
        {
            var response = await _messageService.GetReceivedMessageAsync(id);
            return Response(response);
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<GetSenderMessageViewModel>> GetSentMessage(Guid id)
        {
            var response = await _messageService.GetSentMessageAsync(id);
            return Response(response);
        }

        [HttpPost]
        public async Task<BaseDataResponse<GetSenderMessageViewModel>> Create([FromBody]CreateMessageViewModel messageViewModel)
        {
            var response = await _messageService.SendMessageAsync(messageViewModel);
            return Response(response);
        }
    }
}
