using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Services;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Handbook.UsefulLinkCategory;
using Samr.ERP.Core.ViewModels.UsefulLink;
using Samr.ERP.Core.ViewModels.UsefulLink.UsefulLinkCategory;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.WebApi.Filters;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class UsefulLinkController : ApiController
    {
        private readonly IUsefulLinkService _usefulLinkService;

        public UsefulLinkController(IUsefulLinkService usefulLinkService)
        {
            _usefulLinkService = usefulLinkService;
        }

        [HttpGet]
        public async Task<BaseDataResponse<PagedList<UsefulLinkViewModel>>> All([FromQuery] PagingOptions pagingOptions, [FromQuery]FilterUsefulLinkViewModel filterUsefulLinkViewModel)
        {
            var response = await _usefulLinkService.GetAllAsync(pagingOptions, filterUsefulLinkViewModel);
            return Response(response);
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<UsefulLinkViewModel>> Get(Guid id)
        {
            var response = await _usefulLinkService.GetByIdAsync(id);

            return Response(response);
        }

        [HttpPost]
        [TrimInputStrings]
        public async Task<BaseDataResponse<UsefulLinkViewModel>> Create([FromBody] UsefulLinkViewModel usefulLinkViewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _usefulLinkService.CreateAsync(usefulLinkViewModel);
                return Response(response);
            }

            return Response(BaseDataResponse<UsefulLinkViewModel>.Fail(usefulLinkViewModel, null));
        }

        [HttpPost]
        [TrimInputStrings]
        public async Task<BaseDataResponse<UsefulLinkViewModel>> Edit([FromBody] UsefulLinkViewModel usefulLinkViewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _usefulLinkService.EditAsync(usefulLinkViewModel);
                return Response(response);
            }

            return Response(BaseDataResponse<UsefulLinkViewModel>.Fail(null, null));
        }
    }
}
