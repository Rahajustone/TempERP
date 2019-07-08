﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.News;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class NewsController : ApiController
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }


        [HttpGet]
        public async Task<BaseDataResponse<IEnumerable<EditNewsViewModel>>> All()
        {
            var news = await _newsService.GetAllAsync();
            return Response(news);
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<EditNewsViewModel>> Get(Guid id)
        {
            var news = await _newsService.GetByIdAsync(id);

            return Response(news);
        }

        [HttpPost]
        public async Task<BaseDataResponse<EditNewsViewModel>> Post([FromBody] EditNewsViewModel newsViewModel)
        {
            if (ModelState.IsValid)
            {
                var responseData = await _newsService.CreateAsync(newsViewModel);

                return Response(responseData);
            }

            return Response(BaseDataResponse<EditNewsViewModel>.NotFound(newsViewModel, new ErrorModel("Error")));
        }

        [HttpPost]
        public async Task<BaseDataResponse<EditNewsViewModel>> Edit([FromBody] EditNewsViewModel positionViewModel)
        {
            if (ModelState.IsValid)
            {
                var responseData = await _newsService.UpdateAsync(positionViewModel);
                return Response(responseData);
            }

            return Response(BaseDataResponse<EditNewsViewModel>.Fail(null, null));
        }
    }
}