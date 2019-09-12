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
using Samr.ERP.Core.Staff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Handbook.FileArchiveCategory;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.WebApi.Filters;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class FileArchiveCategoryController : ApiController
    {
        private readonly IFileArchiveCategoryService _fileArchiveCategoryService;

        public FileArchiveCategoryController(IFileArchiveCategoryService fileArchiveCategoryService)
        {
            _fileArchiveCategoryService = fileArchiveCategoryService;
        }

        [HttpGet]
        public async Task<BaseDataResponse<PagedList<ResponseFileArchiveCategoryViewModel>>> All([FromQuery]PagingOptions pagingOptions, [FromQuery]FilterHandbookViewModel filterHandbook, [FromQuery] SortRule sortRule)
        {
            var fileCategories = await _fileArchiveCategoryService.GetAllAsync(pagingOptions, filterHandbook, sortRule);
            return Response(fileCategories);
        }

        [HttpGet]
        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> SelectListItem()
        {
            var listedItem = await _fileArchiveCategoryService.GetAllSelectListItemAsync();
            return Response(listedItem);
        }

        [HttpGet]
        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetCategoriesWithFileArchiveAllSelectListItem()
        {
            var listedItem = await _fileArchiveCategoryService.GetCategoriesWithFileArchiveAllSelectListItemAsync();
            return Response(listedItem);
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<ResponseFileArchiveCategoryViewModel>> Get(Guid id)
        {
            var fileCategory = await _fileArchiveCategoryService.GetByIdAsync(id);

            return Response(fileCategory);
        }

        [HttpPost]
        [TrimInputStrings]
        public async Task<BaseDataResponse<ResponseFileArchiveCategoryViewModel>> Create([FromBody] RequestFileArchiveCategoryViewModel editFileArchiveCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _fileArchiveCategoryService.CreateAsync(editFileArchiveCategoryViewModel);
                return Response(result);
            }

            return Response(BaseDataResponse<ResponseFileArchiveCategoryViewModel>.NotFound(null));
        }

        [HttpPost]
        [TrimInputStrings]
        public async Task<BaseDataResponse<ResponseFileArchiveCategoryViewModel>> Edit([FromBody] RequestFileArchiveCategoryViewModel editFileArchiveCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _fileArchiveCategoryService.EditAsync(editFileArchiveCategoryViewModel);
                return Response(result);
            }

            return Response(BaseDataResponse<ResponseFileArchiveCategoryViewModel>.NotFound(null));
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<PagedList<FileArchiveCategoryLogViewModel>>> GetAllLog(Guid id, [FromQuery] PagingOptions pagingOptions, [FromQuery]SortRule sortRule)
        {
            var response = await _fileArchiveCategoryService.GetAllLogAsync(id, pagingOptions, sortRule);
            return Response(response);
        }
    }
}