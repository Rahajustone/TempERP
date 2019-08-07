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
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Position;

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
        public async Task<BaseDataResponse<PagedList<EditPositionViewModel>>> All([FromQuery]PagingOptions pagingOptions, [FromQuery]FilterPositionViewModel filterPosition, [FromQuery] SortRule sortRule)
        {
            var position = await _positionService.GetAllAsync(pagingOptions, filterPosition, sortRule);
            return Response(position);
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<IEnumerable<PositionViewModel>>> AllByDepartmentId(Guid id)
        {
            var position = await _positionService.GetAllByDepartmentId(id);
            return Response(position);
        }
 
        [HttpGet("{id}")]
        public async Task<BaseDataResponse<EditPositionViewModel>> Get(Guid id)
        {
            var position = await _positionService.GetByIdAsync(id);

            return Response(position);
        }

        [HttpPost]
        public async Task<BaseDataResponse<EditPositionViewModel>> Create([FromBody]EditPositionViewModel positionViewModel)
        {
            if (ModelState.IsValid)
            {
                var position = await _positionService.CreateAsync(positionViewModel);
                return Response(position);
            }

            return Response(BaseDataResponse<EditPositionViewModel>.Fail(positionViewModel, null));
        }

        [HttpPost]
        public async Task<BaseDataResponse<EditPositionViewModel>> Edit([FromBody] EditPositionViewModel positionViewModel)
        {
            if (ModelState.IsValid)
            {
                var positionResult = await _positionService.EditAsync(positionViewModel);
                return Response(positionResult);
            }

            return Response(BaseDataResponse<EditPositionViewModel>.Fail(null, null));
        }

        [HttpGet]
        public async Task<BaseDataResponse<PagedList<PositionLogViewModel>>> GetAllLog(Guid id, PagingOptions pagingOptions, SortRule sortRule)
        {
            var position = await _positionService.GetAllLogAsync(id, pagingOptions, sortRule);
            return Response(position);
        }
    }
}
