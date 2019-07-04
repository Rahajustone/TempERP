using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.News.Categories;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class NewsCategoriesController : ApiController
    {
        private readonly INewsCategoriesService _newsCategoriesService;

        public NewsCategoriesController(INewsCategoriesService newsCategoriesService)
        {
            _newsCategoriesService = newsCategoriesService;
        }

        [HttpGet]
        public async Task<BaseDataResponse<IEnumerable<NewsCategoriesViewModel>>> All()
        {
            var newsCategories = await _newsCategoriesService.GetAllAsync();
            return Response(newsCategories);
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<NewsCategoriesViewModel>> Get(Guid id)
        {
            var newsCategory = await _newsCategoriesService.GetByIdAsync(id);
            return Response(newsCategory);
        }

        [HttpPost]
        public async Task<BaseDataResponse<NewsCategoriesViewModel>> Create([FromBody] NewsCategoriesViewModel newsCategoriesViewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _newsCategoriesService.CreateAsync(newsCategoriesViewModel);

                return response;
            }

            return BaseDataResponse<NewsCategoriesViewModel>.Fail(null);
        }

        [HttpPost]
        public async Task<BaseDataResponse<NewsCategoriesViewModel>> Update([FromBody] NewsCategoriesViewModel newsCategoriesViewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _newsCategoriesService.UpdateAsync(newsCategoriesViewModel);

                return response;
            }

            return BaseDataResponse<NewsCategoriesViewModel>.Fail(null);
        }
    }
}
