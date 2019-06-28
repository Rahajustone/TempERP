using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.Account;
using Samr.ERP.Core.ViewModels.Employee;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeeController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }
        // GET: api/Employee
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Employee/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Employee
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] AddEmployeeViewModel employee)
        {
            var createdEmployee = await _employeeService.CreateAsync(_mapper.Map<Employee>(employee));
            var vm = _mapper.Map<AddEmployeeViewModel>(createdEmployee.Data);
            return Ok(vm);
        }

        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public void Edit(int id, [FromBody] string value)
        {
        }
        
        [HttpPost]
        public async Task<BaseResponse<UserViewModel>> CreateUser([FromBody] Guid employeeId)
        {
            var createdUserResponse = await _employeeService.CreateUserForEmployee(employeeId);

            return Response(createdUserResponse);
        }
    }
}
