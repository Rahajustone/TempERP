using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Services;
using Samr.ERP.Core.ViewModels.Department;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class DepartmentController : ApiController
    {
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentService departmentService, IMapper iMapper)
        {
            _departmentService = departmentService;
            _mapper = iMapper;
        }

        [HttpGet]
        public async Task<ActionResult> All()
        {
            var departments = await _departmentService.GetAll();
            var vm = _mapper.Map<IEnumerable<DepartmentViewModel>>(departments.Data);
            return Ok(vm);
        }

        [HttpGet("{id}")]
        public async Task<BaseResponse<EditDepartmentViewModel>> Get(Guid id)
        {
            var department = await _departmentService.GetByIdAsync(id);
            return Response(department);

        }
        
        [HttpPost]
        public async Task<BaseResponse<EditDepartmentViewModel>> Create([FromBody]EditDepartmentViewModel departmentViewModel)
        {
            if (ModelState.IsValid)
            {
                var departmentResult = await _departmentService.CreateAsync(departmentViewModel);
                return Response(departmentResult);
            }

            return Response(BaseResponse<EditDepartmentViewModel>.Fail(departmentViewModel, null));
        }

        // PUT: api/Department/5
        [HttpPost()]
        public async Task<BaseResponse<EditDepartmentViewModel>> Edit([FromBody] EditDepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var departmentResult = await _departmentService.UpdateAsync(model);
                return Response(departmentResult);
            }

            return Response(BaseResponse<EditDepartmentViewModel>.Fail(null, null));
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<BaseResponse<DepartmentViewModel>> Delete(Guid id)
        {
            if (ModelState.IsValid)
            {
                var departmentResult = await _departmentService.DeleteAsync(id);
                return Response(departmentResult);
            }

            return Response(BaseResponse<DepartmentViewModel>.Fail(null, null));
        }
    }
}
