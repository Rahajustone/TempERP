using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Handbook.EmployeeLockReason;

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
        public async Task<BaseDataResponse<PagedList<EditEmployeeLockReasonViewModel>>> All([FromQuery]PagingOptions pagingOptions, [FromQuery]FilterHandbookViewModel filterHandbook, [FromQuery] SortRule sortRule)
        {
            var employeeLockReasons = await _employeeLockReason.GetAllAsync(pagingOptions, filterHandbook, sortRule);
            return Response(employeeLockReasons);
        }

        [HttpGet]
        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllListItems()
        {
            var listItem = await _employeeLockReason.GetAllListItemAsync();

            return Response(listItem);
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
        public async Task<BaseDataResponse<EditEmployeeLockReasonViewModel>> Edit([FromBody] EditEmployeeLockReasonViewModel employeeLockReasonViewModel)
        {
            if (ModelState.IsValid)
            {
                var departmentResult = await _employeeLockReason.EditAsync(employeeLockReasonViewModel);
                return Response(departmentResult);
            }

            return Response(BaseDataResponse<EditEmployeeLockReasonViewModel>.Fail(null, null));
        }
    }
}
