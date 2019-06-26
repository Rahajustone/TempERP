using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.Department;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

        public async Task<BaseResponse<DepartmentViewModel>> CreateAsync(DepartmentViewModel department)
        {
            var vm = _mapper.Map<Department>(department);

            var departmentResult = await _unitOfWork.Departments.AddAsync(vm);

            _unitOfWork.Commit();

            var response = BaseResponse<DepartmentViewModel>.Success(_mapper.Map<DepartmentViewModel>(vm), null);

            return response;
        }

        public async Task<BaseResponse<DepartmentViewModel>> UpdateAsync(DepartmentViewModel model)
        {
            var department = _mapper.Map<Department>(model);
             _unitOfWork.Departments.Update(department);

            _unitOfWork.Commit();

            var response = BaseResponse<DepartmentViewModel>.Success(_mapper.Map<DepartmentViewModel>(department), null);

            return response;

        }

        public async Task<BaseResponse<DepartmentViewModel>> DeleteAsync(Guid id)
        {
            var department = _unitOfWork.Departments.GetByIdAsync(id);
            
            var vm = _mapper.Map<Department>(department);
            _unitOfWork.Departments.Delete(vm);

            _unitOfWork.Commit();

            var response = BaseResponse<DepartmentViewModel>.Success(_mapper.Map<DepartmentViewModel>(vm), null);

            return response;
        }
    }
}