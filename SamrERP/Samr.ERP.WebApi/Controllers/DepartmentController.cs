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
using Samr.ERP.Core.ViewModels.Department;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class DepartmentController : ApiController
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<BaseDataResponse<PagedList<EditDepartmentViewModel>>> All([FromQuery]PagingOptions pagingOptions, [FromQuery]FilterHandbookViewModel filterHandbook, [FromQuery] SortRule sortRule)
        {
            var departments = await _departmentService.GetAllAsync(pagingOptions, filterHandbook, sortRule);
            return Response(departments);
        }

        [HttpGet]
        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> SelectListItem()
        {
            var departments = await _departmentService.GetAllSelectListItemAsync();
            return Response(departments);
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<EditDepartmentViewModel>> Get(Guid id)
        {
            var department = await _departmentService.GetByIdAsync(id);

            return Response(department);
        }
        
        [HttpPost]
        public async Task<BaseDataResponse<EditDepartmentViewModel>> Create([FromBody]EditDepartmentViewModel departmentViewModel)
        {
            if (ModelState.IsValid)
            {
                var departmentResult = await _departmentService.CreateAsync(departmentViewModel);
                return Response(departmentResult);
            }

            return Response(BaseDataResponse<EditDepartmentViewModel>.Fail(departmentViewModel, null));
        }

        [HttpPost]
        public async Task<BaseDataResponse<EditDepartmentViewModel>> Edit([FromBody] EditDepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var departmentResult = await _departmentService.UpdateAsync(model);
                return Response(departmentResult);
            }

            return Response(BaseDataResponse<EditDepartmentViewModel>.Fail(null, null));
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<PagedList<DepartmentLogViewModel>>> GetAllLog(Guid id, [FromQuery]PagingOptions pagingOptions, [FromQuery] SortRule sortRule)
        {
            var departmentsLog = await _departmentService.GetAllLogAsync(id, pagingOptions, sortRule);
            return Response(departmentsLog);
        }
    }
}
