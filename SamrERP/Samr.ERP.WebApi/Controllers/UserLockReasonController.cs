using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Staff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Handbook.UserLockReason;
using Samr.ERP.WebApi.Filters;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class UserLockReasonController : ApiController
    {
        private readonly IUserLockReasonService _userLockReasonService;

        public UserLockReasonController(IUserLockReasonService userLockReasonService)
        {
            _userLockReasonService = userLockReasonService;
        }

        [HttpGet]
        public async Task<BaseDataResponse<PagedList<ResponseUserLockReasonViewModel>>> All([FromQuery]PagingOptions pagingOptions, [FromQuery]FilterHandbookViewModel filterHandbook, [FromQuery] SortRule sortRule)
        {
            var userLockReasons = await _userLockReasonService.GetAllAsync(pagingOptions, filterHandbook, sortRule);
            return Response(userLockReasons);
        }

        [HttpGet]
        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllListItems()
        {
            var listItems = await _userLockReasonService.GetAllListItemsAsync();

            return Response(listItems);
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<ResponseUserLockReasonViewModel>> Get(Guid id)
        {
            var userLockReasons = await _userLockReasonService.GetByIdAsync(id);

            return Response(userLockReasons);
        }

        [HttpPost]
        [TrimInputStrings]
        public async Task<BaseDataResponse<ResponseUserLockReasonViewModel>> Create(RequestUserLockReasonViewModel userLockReasonViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _userLockReasonService.CreateAsync(userLockReasonViewModel);

                return Response(result);
            }

            return Response(BaseDataResponse<ResponseUserLockReasonViewModel>.NotFound(null));
        }

        [HttpPost]
        [TrimInputStrings]
        public async Task<BaseDataResponse<ResponseUserLockReasonViewModel>> Edit([FromBody] RequestUserLockReasonViewModel userLockReasonViewModel)
        {
            if (ModelState.IsValid)
            {
                var responseData = await _userLockReasonService.EditAsync(userLockReasonViewModel);
                return Response(responseData);
            }

            return Response(BaseDataResponse<ResponseUserLockReasonViewModel>.Fail(null));
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<PagedList<UserLockReasonLogViewModel>>> GetAllLog(Guid id, [FromQuery] PagingOptions pagingOptions, [FromQuery]SortRule sortRule)
        {
            var response = await _userLockReasonService.GetAllLogAsync(id, pagingOptions, sortRule);
            return Response(response);
        }
    }
}