using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
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
        public async Task<BaseResponse<IEnumerable<EditNationalityViewModel>>> All()
        {
            var nationalities = await _nationalityService.GetAllAsync();
            return Response(nationalities);
        }

        [HttpGet("{id}")]
        public async Task<BaseResponse<EditNationalityViewModel>> Get(Guid id)
        {
            var nationality = await _nationalityService.GetByIdAsync(id);

            return nationality;
        }

        // POST: api/Nationality
        [HttpPost]
        public  async Task<BaseResponse<EditNationalityViewModel>> Create([FromBody]EditNationalityViewModel nationalityViewModel)
        {
            if (ModelState.IsValid)
            {
                var nationality = await _nationalityService.CreateAsync(nationalityViewModel);
                return Response(nationality);
            }

            return Response(BaseResponse<EditNationalityViewModel>.Fail(nationalityViewModel,
                new ErrorModel("Model Not created")));
        }


        [HttpPost]
        public async  Task<BaseResponse<EditNationalityViewModel>> Edit([FromBody]EditNationalityViewModel nationalityViewModel)
        {
            if (ModelState.IsValid)
            {
                var nationality = await _nationalityService.UpdateAsync(nationalityViewModel);

                return Response(nationality);
            }

            return Response(BaseResponse<EditNationalityViewModel>.Fail(nationalityViewModel, null));
        }

    }
}
