using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public async Task<BaseResponse<EditDepartmentViewModel>> GetByIdAsync(Guid id)
        {
            var departmentResult = await _unitOfWork.Departments.GetByIdAsync(id);
            var firstOrDefault = _unitOfWork.Departments.GetDbSet().Include(p => p.CreatedUser).FirstOrDefault(p => p.Id == id);

            var vm = _mapper.Map<EditDepartmentViewModel>(departmentResult);

            var response = new BaseResponse<EditDepartmentViewModel>(vm, true);

            return response;
        }

        public async Task<BaseResponse<IEnumerable<DepartmentViewModel>>> GetAll()
        {
            //var department = _unitOfWork.Departments.GetAll();

            var department = _unitOfWork.Departments.GetDbSet().Include(u => u.CreatedUser).ToList();
            var vm = _mapper.Map<IEnumerable<DepartmentViewModel>>(department);

            var response = new BaseResponse<IEnumerable<DepartmentViewModel>>(vm, true);

            return response;
        }

        public async Task<BaseResponse<EditDepartmentViewModel>> CreateAsync(EditDepartmentViewModel departmentViewModel)
        {
            var department = _mapper.Map<Department>(departmentViewModel);

            _unitOfWork.Departments.Add(department);

            await _unitOfWork.CommitAsync();

            var response = BaseResponse<EditDepartmentViewModel>.Success(_mapper.Map<EditDepartmentViewModel>(department), null);

            return response;
        }

        public async Task<BaseResponse<EditDepartmentViewModel>> UpdateAsync(EditDepartmentViewModel model)
        {
            var department = _mapper.Map<Department>(model);
             _unitOfWork.Departments.Update(department);

            await _unitOfWork.CommitAsync();

            var response = BaseResponse<EditDepartmentViewModel>.Success(_mapper.Map<EditDepartmentViewModel>(department), null);

            return response;

        }

        public async Task<BaseResponse<DepartmentViewModel>> DeleteAsync(Guid id)
        {
            var department = _unitOfWork.Departments.GetByIdAsync(id);
            
            var vm = _mapper.Map<Department>(department);
            _unitOfWork.Departments.Delete(vm);

            await _unitOfWork.CommitAsync();

            var response = BaseResponse<DepartmentViewModel>.Success(_mapper.Map<DepartmentViewModel>(vm), null);

            return response;
        }
    }
}