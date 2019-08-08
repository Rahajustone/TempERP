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
        public async Task<BaseDataResponse<PagedList<NewsCategoriesViewModel>>> All([FromQuery]PagingOptions pagingOptions, [FromQuery]FilterHandbookViewModel filterHandbook, [FromQuery] SortRule sortRule)
        {
            var newsCategories = await _newsCategoryService.GetAllAsync(pagingOptions, filterHandbook, sortRule);
            return Response(newsCategories);
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<NewsCategoriesViewModel>> Get(Guid id)
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
        public async Task<BaseDataResponse<NewsCategoriesViewModel>> Create([FromBody] NewsCategoriesViewModel newsCategoriesViewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _newsCategoryService.CreateAsync(newsCategoriesViewModel);

                return response;
            }

            return BaseDataResponse<NewsCategoriesViewModel>.Fail(null);
        }

        [HttpPost]
        public async Task<BaseDataResponse<NewsCategoriesViewModel>> Edit([FromBody] NewsCategoriesViewModel newsCategoriesViewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _newsCategoryService.EditAsync(newsCategoriesViewModel);

                return response;
            }

            return BaseDataResponse<NewsCategoriesViewModel>.Fail(null);
        }
    }
}
