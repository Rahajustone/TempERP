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
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Handbook.FileCategory;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class FileCategoryController : ApiController
    {
        private readonly IFileCategoryService _fileCategoryService;

        public FileCategoryController(IFileCategoryService fileCategoryService)
        {
            _fileCategoryService = fileCategoryService;
        }

        [HttpGet]
        public async Task<BaseDataResponse<PagedList<EditFileCategoryViewModel>>> All([FromQuery]PagingOptions pagingOptions, [FromQuery]FilterHandbookViewModel filterHandbook, [FromQuery] SortRule sortRule)
        {
            var fileCategories = await _fileCategoryService.GetAllAsync(pagingOptions, filterHandbook, sortRule);
            return Response(fileCategories);
        }

        [HttpGet]
        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> SelectListItem()
        {
            var listedItem = await _fileCategoryService.GetAllSelectListItemAsync();
            return Response(listedItem);
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<EditFileCategoryViewModel>> Get(Guid id)
        {
            var fileCategory = await _fileCategoryService.GetByIdAsync(id);

            return Response(fileCategory);
        }

        [HttpPost]
        public async Task<BaseDataResponse<EditFileCategoryViewModel>> Create([FromBody] EditFileCategoryViewModel editFileCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _fileCategoryService.CreateAsync(editFileCategoryViewModel);
                return Response(result);
            }

            return Response(BaseDataResponse<EditFileCategoryViewModel>.Fail(editFileCategoryViewModel, null));
        }

        [HttpPost]
        public async Task<BaseDataResponse<EditFileCategoryViewModel>> Edit([FromBody] EditFileCategoryViewModel editFileCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _fileCategoryService.EditAsync(editFileCategoryViewModel);
                return Response(result);
            }

            return Response(BaseDataResponse<EditFileCategoryViewModel>.Fail(null, null));
        }

    }
}