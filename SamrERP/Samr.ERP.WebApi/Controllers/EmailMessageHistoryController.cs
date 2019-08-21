using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.EmailSetting;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class EmailMessageHistoryController : ApiController
    {
        private readonly IEmailMessageHistoryService _emailMessageHistory;

        public EmailMessageHistoryController(IEmailMessageHistoryService emailMessageHistory)
        {
            _emailMessageHistory = emailMessageHistory;
        }

        [HttpGet]
        public async Task<BaseDataResponse<PagedList<EmailMessageHistoryLogViewModel>>> GetAllLog([FromQuery] PagingOptions pagingOptions, [FromQuery]SortRule sortRule, [FromQuery]FilterEmailMessageHistoryLogViewModel filterEmailMessageHistoryLogViewModel)
        {
            var response = await _emailMessageHistory.GetAllLogAsync(pagingOptions, sortRule, filterEmailMessageHistoryLogViewModel);
            return Response(response);
        }
    }
}
