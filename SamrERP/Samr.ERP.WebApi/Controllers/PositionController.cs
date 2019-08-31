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
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Position;
using Samr.ERP.WebApi.Filters;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class PositionController : ApiController
    {
        private readonly IPositionService _positionService;

        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        [HttpGet]
        public async Task<BaseDataResponse<PagedList<ResponsePositionViewModel>>> All([FromQuery]PagingOptions pagingOptions, [FromQuery]FilterPositionViewModel filterPosition, [FromQuery] SortRule sortRule)
        {
            var position = await _positionService.GetAllAsync(pagingOptions, filterPosition, sortRule);
            return Response(position);
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> AllByDepartmentId(Guid id)
        {
            var position = await _positionService.GetAllByDepartmentId(id);
            return Response(position);
        }
 
        [HttpGet("{id}")]
        public async Task<BaseDataResponse<ResponsePositionViewModel>> Get(Guid id)
        {
            var position = await _positionService.GetByIdAsync(id);

            return Response(position);
        }

        [HttpPost]
        [TrimInputStrings]
        public async Task<BaseDataResponse<ResponsePositionViewModel>> Create([FromBody]RequestPositionViewModel positionViewModel)
        {
            if (ModelState.IsValid)
            {
                var position = await _positionService.CreateAsync(positionViewModel);
                return Response(position);
            }

            return Response(BaseDataResponse<ResponsePositionViewModel>.NotFound(null));
        }

        [HttpPost]
        [TrimInputStrings]
        public async Task<BaseDataResponse<ResponsePositionViewModel>> Edit([FromBody] RequestPositionViewModel positionViewModel)
        {
            if (ModelState.IsValid)
            {
                var positionResult = await _positionService.EditAsync(positionViewModel);
                return Response(positionResult);
            }

            return Response(BaseDataResponse<ResponsePositionViewModel>.NotFound(null));
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<PagedList<PositionLogViewModel>>> GetAllLog(Guid id, [FromQuery]PagingOptions pagingOptions, [FromQuery]SortRule sortRule)
        {
            var position = await _positionService.GetAllLogAsync(id, pagingOptions, sortRule);
            return Response(position);
        }
    }
}
