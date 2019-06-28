using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.Department;
using Samr.ERP.Core.ViewModels.Handbook;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class EmployeeLockReasonController : ApiController
    {
        private readonly IEmployeeLockReasonService _employeeLockReason;

        public EmployeeLockReasonController(IEmployeeLockReasonService  employeeLockReason)
        {
            _employeeLockReason = employeeLockReason;
        }

        [HttpGet]
        public async Task<BaseDataResponse<IEnumerable<EmployeeLockReasonViewModel>>> All()
        {
            var employeeLockReasons = await _employeeLockReason.GetAll();
            return Response(employeeLockReasons);
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<EditEmployeeLockReasonViewModel>> Get(Guid id)
        {
            var employeeLockReason = await _employeeLockReason.GetByIdAsync(id);

            return Response(employeeLockReason);
        }

        [HttpPost]
        public async Task<BaseDataResponse<EditEmployeeLockReasonViewModel>> Create([FromBody]EditEmployeeLockReasonViewModel employeeLockReasonViewModel)
        {
            if (ModelState.IsValid)
            {
                var employeeLockResult = await _employeeLockReason.CreateAsync(employeeLockReasonViewModel);
                return Response(employeeLockResult);
            }

            return Response(BaseDataResponse<EditEmployeeLockReasonViewModel>.Fail(employeeLockReasonViewModel, null));
        }

        [HttpPost]
        public async Task<BaseDataResponse<EditEmployeeLockReasonViewModel>> Edit(Guid id, [FromBody] EditEmployeeLockReasonViewModel employeeLockReasonViewModel)
        {
            if (ModelState.IsValid)
            {
                var departmentResult = await _employeeLockReason.UpdateAsync(employeeLockReasonViewModel);
                return Response(departmentResult);
            }

            return Response(BaseDataResponse<EditEmployeeLockReasonViewModel>.Fail(null, null));
        }
    }
}
