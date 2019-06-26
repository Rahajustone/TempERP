using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        // GET: api/Department
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var department = await _departmentService.GetByIdAsync(id);
            var vm = _mapper.Map<DepartmentViewModel>(department.Data);
            return Ok(vm);
        }

        // POST: api/Department
        [HttpPost]
        public async Task<BaseResponse<DepartmentViewModel>> Create([FromBody]DepartmentViewModel department)
        {
            if (ModelState.IsValid)
            {
                var departmentResult = await _departmentService.CreateAsync(department);
                return Response(departmentResult);
            }

            return Response(BaseResponse<DepartmentViewModel>.Fail(null, null));
        }

        // PUT: api/Department/5
        [HttpPost("{id}")]
        public async Task<BaseResponse<DepartmentViewModel>> Edit(Guid id, [FromBody] DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var departmentResult = await _departmentService.UpdateAsync(model);
                return Response(departmentResult);
            }

            return Response(BaseResponse<DepartmentViewModel>.Fail(null, null));
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
