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
using Samr.ERP.Core.ViewModels.Handbook.NewCategories;
using Samr.ERP.WebApi.Filters;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class NewsCategoryController : ApiController
    {
        private readonly INewsCategoryService _newsCategoryService;

        public NewsCategoryController(INewsCategoryService newsCategoryService)
        {
            _newsCategoryService = newsCategoryService;
        }

        [HttpGet]
        public async Task<BaseDataResponse<PagedList<ResponseNewsCategoryViewModel>>> All([FromQuery]PagingOptions pagingOptions, [FromQuery]FilterHandbookViewModel filterHandbook, [FromQuery] SortRule sortRule)
        {
            var newsCategories = await _newsCategoryService.GetAllAsync(pagingOptions, filterHandbook, sortRule);
            return Response(newsCategories);
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<ResponseNewsCategoryViewModel>> Get(Guid id)
        {
            var newsCategory = await _newsCategoryService.GetByIdAsync(id);
            return Response(newsCategory);
        }

        [HttpGet]
        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> SelectListItem()
        {
            var listedItem = await _newsCategoryService.GetAllSelectListItemAsync();
            return Response(listedItem);
        }

        [HttpPost]
        [TrimInputStrings]
        public async Task<BaseDataResponse<ResponseNewsCategoryViewModel>> Create([FromBody] RequestNewsCategoryViewModel responseNewsCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _newsCategoryService.CreateAsync(responseNewsCategoryViewModel);

                return response;
            }

            return BaseDataResponse<ResponseNewsCategoryViewModel>.Fail(null);
        }

        [HttpPost]
        [TrimInputStrings]
        public async Task<BaseDataResponse<ResponseNewsCategoryViewModel>> Edit([FromBody] RequestNewsCategoryViewModel requestNewsCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _newsCategoryService.EditAsync(requestNewsCategoryViewModel);

                return response;
            }

            return BaseDataResponse<ResponseNewsCategoryViewModel>.Fail(null);
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<PagedList<NewsCategoryLogViewModel>>> GetAllLog(Guid id, [FromQuery] PagingOptions pagingOptions, [FromQuery]SortRule sortRule)
        {
            var response = await _newsCategoryService.GetAllLogAsync(id, pagingOptions, sortRule);
            return Response(response);
        }
    }
}
