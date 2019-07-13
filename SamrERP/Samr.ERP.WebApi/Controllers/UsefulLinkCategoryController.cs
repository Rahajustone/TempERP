﻿using System;
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
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.UsefulLink.UsefulLinkCategory;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UsefulLinkCategoryController : ApiController
    {
        private readonly IUsefulLinkCategoryService _usefulLinkCategoryService;

        public UsefulLinkCategoryController(IUsefulLinkCategoryService usefulLinkCategoryService)
        {
            _usefulLinkCategoryService = usefulLinkCategoryService;
        }

        [HttpGet]
        public async Task<BaseDataResponse<PagedList<EditUsefulLinkCategoryViewModel>>> All([FromQuery]PagingOptions pagingOptions)
        {
            var usefulLinCategories = await _usefulLinkCategoryService.GetAllAsync(pagingOptions);
            return Response(usefulLinCategories);
        }

        [HttpGet]
        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllListItems()
        {
            var listItems = await _usefulLinkCategoryService.GetAllSelectListItemAsync();

            return Response(listItems);
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<EditUsefulLinkCategoryViewModel>> Get(Guid id)
        {
            var usefulLinCategory = await _usefulLinkCategoryService.GetByIdAsync(id);

            return Response(usefulLinCategory);
        }


        [HttpPost]
        public async Task<BaseDataResponse<EditUsefulLinkCategoryViewModel>> Create([FromBody] EditUsefulLinkCategoryViewModel editUsefulLinkCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _usefulLinkCategoryService.CreateAsync(editUsefulLinkCategoryViewModel);

                return Response(response);
            }

            return Response(BaseDataResponse<EditUsefulLinkCategoryViewModel>.NotFound(editUsefulLinkCategoryViewModel));
        }

        [HttpPost]
        public async Task<BaseDataResponse<EditUsefulLinkCategoryViewModel>> Edit([FromBody] EditUsefulLinkCategoryViewModel editUsefulLinkCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _usefulLinkCategoryService.EditAsync(editUsefulLinkCategoryViewModel);

                return Response(response);
            }

            return Response(BaseDataResponse<EditUsefulLinkCategoryViewModel>.NotFound(editUsefulLinkCategoryViewModel));
        }
    }
}