using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ResponseModels;
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
        private readonly IHostingEnvironment _host;

        public EmployeeController(IEmployeeService employeeService, IHostingEnvironment host)
        {
            _employeeService = employeeService;
            _host = host;
        }

        [HttpGet]
        public  async  Task<BaseResponse<IEnumerable<Employee>>> All()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public async Task<BaseDataResponse<EditEmployeeViewModel>> Create([FromForm] EditEmployeeViewModel editEmployeeViewModel, IFormFile photoFile)
        {
            if (ModelState.IsValid)
            {
                var employeeResult =  await _employeeService.CreateAsync(editEmployeeViewModel, photoFile);

                return Response(employeeResult);
            }

            return Response(BaseDataResponse<EditEmployeeViewModel>.Fail(editEmployeeViewModel, null));

        }

        [HttpPost]
        public void Edit([FromBody] string value)
        {
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

        [HttpPost]
        public async Task<BaseResponse> UnLockEmployee([FromBody] Guid id)
        {
            if (ModelState.IsValid)
            {
                var response = await _employeeService.UnLockEmployeeAsync(id);
                return Response(response);
            }
            return Response(BaseResponse.Fail());
        }
    }
}
