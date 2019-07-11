using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Account;
using Samr.ERP.Core.ViewModels.Employee;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService, IHostingEnvironment host)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<BaseDataResponse<PagedList<AllEmployeeViewModel>>> All([FromQuery]PagingOptions pagingOptions, [FromQuery]FilterEmployeeViewModel filterEmployeeViewModel)
        {
            
            var employee = await _employeeService.AllAsync(pagingOptions, filterEmployeeViewModel);

            return Response(employee);
        }

        [HttpGet]
        public async Task<BaseDataResponse<PagedList<AllLockEmployeeViewModel>>> AllLockedEmployees([FromQuery]PagingOptions pagingOptions)
        {
            var employee = await _employeeService.GetAllLockedEmployeeAsync(pagingOptions);

            return Response(employee);
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<GetEmployeeViewModel>> Get(Guid id)
        {
            var employee = await _employeeService.GetByIdAsync(id);

            return Response(employee);
        }

        [HttpPost]
        public async Task<BaseDataResponse<EditEmployeeViewModel>> Create([FromBody] EditEmployeeViewModel editEmployeeViewModel)
        {
            if (ModelState.IsValid)
            {
                var employeeResult =  await _employeeService.CreateAsync(editEmployeeViewModel);

                return Response(employeeResult);
            }

            return Response(BaseDataResponse<EditEmployeeViewModel>.Fail(editEmployeeViewModel, null));

        }

        [HttpPost]
        public async Task<BaseDataResponse<EditEmployeeViewModel>> Edit(
            [FromBody] EditEmployeeViewModel editEmployeeViewModel)
        {
            if (ModelState.IsValid)
            {
                var employeeResult = await _employeeService.EditAsync(editEmployeeViewModel);
                return Response(employeeResult);
            }

            return Response(BaseDataResponse<EditEmployeeViewModel>.Fail(editEmployeeViewModel, null));
        }

        [HttpPost]
        public async Task<BaseDataResponse<UserViewModel>> CreateUser([FromBody] Guid employeeId)
        {
            var createdUserResponse = await _employeeService.CreateUserForEmployee(employeeId);

            return Response(createdUserResponse);
        }

        [HttpPost]
        public async Task<BaseResponse> LockEmployee([FromBody] LockEmployeeViewModel lockEmployeeViewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _employeeService.LockEmployeeAsync(lockEmployeeViewModel);
                return Response(response);
            }
            return Response(BaseResponse.Fail());
        }

        [HttpPost("{id}")]
        public async Task<BaseResponse> UnlockEmployee(Guid id)
        {
            if (ModelState.IsValid)
            {
                var response = await _employeeService.UnLockEmployeeAsync(id);
                return Response(response);
            }
            return Response(BaseResponse.Fail());
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<GetPassportDataEmployeeViewModel>> GetPassportData(Guid id)
        {
            var passportData = await _employeeService.GetPassportDataAsync(id);

            return Response(passportData);
        }

        [HttpPost]
        public async Task<BaseResponse> EditPassportData(
            EditPassportDataEmployeeViewModel editPassportDataEmployeeViewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _employeeService.EditPassportDataAsync(editPassportDataEmployeeViewModel);

                return Response(response);
            }

            return Response(BaseResponse.Fail());
        }
    }
}
