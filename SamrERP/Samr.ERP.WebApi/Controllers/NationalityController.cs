using System;
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
using Samr.ERP.Core.ViewModels.Employee;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Handbook.Nationality;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class NationalityController : ApiController
    {
        private readonly INationalityService _nationalityService;

        public NationalityController(INationalityService nationalityService)
        {
            _nationalityService = nationalityService;
        }

        [HttpGet]
        public async Task<BaseDataResponse<PagedList<EditNationalityViewModel>>> All([FromQuery]PagingOptions pagingOptions, [FromQuery]FilterHandbookViewModel filterHandbook, [FromQuery] SortRule sortRule)
        {
            var nationalities = await _nationalityService.GetAllAsync(pagingOptions, filterHandbook, sortRule);
            return Response(nationalities);
        }

        [HttpGet]
        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> SelectListItem()
        {
            var listItems = await _nationalityService.GetAllListItemAsync();

            return Response(listItems);
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<EditNationalityViewModel>> Get(Guid id)
        {
            var nationality = await _nationalityService.GetByIdAsync(id);

            return nationality;
        }

        // POST: api/Nationality
        [HttpPost]
        public  async Task<BaseDataResponse<EditNationalityViewModel>> Create([FromBody]EditNationalityViewModel nationalityViewModel)
        {
            if (ModelState.IsValid)
            {
                var nationality = await _nationalityService.CreateAsync(nationalityViewModel);
                return Response(nationality);
            }

            return Response(BaseDataResponse<EditNationalityViewModel>.Fail(nationalityViewModel,
                new ErrorModel("Model Not created")));
        }


        [HttpPost]
        public async  Task<BaseDataResponse<EditNationalityViewModel>> Edit([FromBody]EditNationalityViewModel nationalityViewModel)
        {
            if (ModelState.IsValid)
            {
                var nationality = await _nationalityService.UpdateAsync(nationalityViewModel);

                return Response(nationality);
            }

            return Response(BaseDataResponse<EditNationalityViewModel>.Fail(nationalityViewModel, null));
        }

        [HttpGet]
        public async Task<BaseDataResponse<PagedList<NationalityLogViewModel>>> GetAllLog([FromQuery]Guid id, [FromQuery] PagingOptions pagingOptions, [FromQuery]SortRule sortRule)
        {
            var nationality = await _nationalityService.GetAllLogAsync(id, pagingOptions, sortRule);
            return Response(nationality);
        }
    }
}
