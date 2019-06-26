﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Infrastructure.Data;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseResponse<Employee>> CreateAsync(Employee employee)
        {
            var employeResult = await _unitOfWork.Employees.AddAsync(employee);

            _unitOfWork.Commit();

            var response = new BaseResponse<Employee>(employeResult, true);

            return response;
        }

        //public IEnumerable<Employee> GetAllUser()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
