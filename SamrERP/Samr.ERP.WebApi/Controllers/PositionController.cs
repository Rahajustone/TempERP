using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ResponseModels;
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
        public async Task<BaseResponse<IEnumerable<PositionViewModel>>> All()
        {
            var position = await _positionService.GetAllAsync();
            return Response(position);
        }

        [HttpGet("{id}")]
        public async Task<BaseResponse<EditPositionViewModel>> Get(Guid id)
        {
            var position = await _positionService.GetByIdAsync(id);

            return Response(position);
        }

        [HttpPost]
        public async Task<BaseResponse<EditPositionViewModel>> Create([FromBody]EditPositionViewModel positionViewModel)
        {
            if (ModelState.IsValid)
            {
                var position = await _positionService.CreateAsync(positionViewModel);
                return Response(position);
            }

            return Response(BaseResponse<EditPositionViewModel>.Fail(positionViewModel, null));
        }

        [HttpPost]
        public async Task<BaseResponse<EditPositionViewModel>> Edit(Guid id, [FromBody] EditPositionViewModel positionViewModel)
        {
            if (ModelState.IsValid)
            {
                var positionResult = await _positionService.UpdateAsync(positionViewModel);
                return Response(positionResult);
            }

            return Response(BaseResponse<EditPositionViewModel>.Fail(null, null));
        }
    }
}
