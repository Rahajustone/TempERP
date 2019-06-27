using System;
using System.Collections.Generic;
using System.Globalization;
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
            var department = await _unitOfWork.Departments.GetDbSet()
                .Include(p => p.CreatedUser)
                .FirstOrDefaultAsync(p => p.Id == id);
            BaseResponse<EditDepartmentViewModel> response;
            if (department == null)
            {
                response = BaseResponse<EditDepartmentViewModel>.NotFound(null);
            }
            else
            {
                response = BaseResponse<EditDepartmentViewModel>.Success(_mapper.Map<EditDepartmentViewModel>(department));
            }
            
            return response;
        }

        public async Task<BaseResponse<IEnumerable<DepartmentViewModel>>> GetAllAsync()
        {
            var department = await _unitOfWork.Departments.GetDbSet().Include(u => u.CreatedUser).ToListAsync();
            var vm = _mapper.Map<IEnumerable<DepartmentViewModel>>(department);

            var response = BaseResponse<IEnumerable<DepartmentViewModel>>.Success(vm);

            return response;
        }

        public async Task<BaseResponse<EditDepartmentViewModel>> CreateAsync(EditDepartmentViewModel departmentViewModel)
        {
            var departmentExists =
                _unitOfWork.Departments.Any(p => p.Name.ToLower() == departmentViewModel.Name.ToLower());
            var department = _mapper.Map<Department>(departmentViewModel);

            _unitOfWork.Departments.Add(department);

            await _unitOfWork.CommitAsync();

            var response = BaseResponse<EditDepartmentViewModel>.Success(_mapper.Map<EditDepartmentViewModel>(department));

            return response;
        }

        public async Task<BaseResponse<EditDepartmentViewModel>> UpdateAsync(EditDepartmentViewModel model)
        {
            BaseResponse<EditDepartmentViewModel> response;
            var departmentExists = await _unitOfWork.Departments.ExistsAsync(model.Id);
            if (departmentExists)
            {
                var department = _mapper.Map<Department>(model);

                _unitOfWork.Departments.Update(department);

                await _unitOfWork.CommitAsync();

                response = BaseResponse<EditDepartmentViewModel>.Success(_mapper.Map<EditDepartmentViewModel>(department));
            }
            else
            {
                response = BaseResponse<EditDepartmentViewModel>.NotFound(model);
            }

            return response;
        }

        public async Task<BaseResponse<DepartmentViewModel>> DeleteAsync(Guid id)
        {
            var department = _unitOfWork.Departments.GetByIdAsync(id);

            var vm = _mapper.Map<Department>(department);
            _unitOfWork.Departments.Delete(vm);

            _unitOfWork.Departments.Delete(department);
            await _unitOfWork.CommitAsync();

            var vm = _mapper.Map<Department>(department);

            var response = BaseResponse<DepartmentViewModel>.Success(_mapper.Map<DepartmentViewModel>(vm));

            return response;
        }
    }
}