using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.Department;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<Department>> GetByIdAsync(Guid id)
        {
            var departmentResult = await _unitOfWork.Departments.GetByIdAsync(id);

            _unitOfWork.Commit();

            var response = new BaseResponse<Department>(departmentResult, true);

            return response;
        }

        public async Task<BaseResponse<IEnumerable<Department>>> GetAll()
        {
            var departmentResult = _unitOfWork.Departments.GetAll();

            var response = new BaseResponse<IEnumerable<Department>>(departmentResult, true);

            return response;
        }

        public async Task<BaseResponse<Department>> CreateAsync(Department department)
        {
            var departmentResult = await _unitOfWork.Departments.AddAsync(department);

            _unitOfWork.Commit();

            var response = new BaseResponse<Department>(departmentResult, true);

            return response;
        }
    }
}