using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
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

        public async Task<BaseDataResponse<EditDepartmentViewModel>> GetByIdAsync(Guid id)
        {
            var department = await _unitOfWork.Departments.GetDbSet()
                .Include(p => p.CreatedUser)
                .FirstOrDefaultAsync(p => p.Id == id);

            BaseDataResponse<EditDepartmentViewModel> dataResponse;

            if (department == null)
            {
                dataResponse = BaseDataResponse<EditDepartmentViewModel>.NotFound(null);
            }
            else
            {
                dataResponse = BaseDataResponse<EditDepartmentViewModel>.Success(_mapper.Map<EditDepartmentViewModel>(department));
            }
            
            return dataResponse;
        }

        public async Task<BaseDataResponse<PagedList<DepartmentViewModel>>> GetAllAsync(PagingOptions pagingOptions)
        {
            var query = _unitOfWork.Departments
                .GetDbSet()
                .Include(u => u.CreatedUser);

            var pagedList = await query.ToMappedPagedListAsync<Department, DepartmentViewModel>(pagingOptions);

            return BaseDataResponse<PagedList<DepartmentViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<EditDepartmentViewModel>> CreateAsync(EditDepartmentViewModel departmentViewModel)
        {
            BaseDataResponse<EditDepartmentViewModel> dataResponse;

            var departmentExists = await CheckDepartmentNameUnique(departmentViewModel.Name);
            if (departmentExists)
            {
                dataResponse = BaseDataResponse<EditDepartmentViewModel>.Fail(departmentViewModel, new ErrorModel("Already this model in database."));
            }
            else
            {
                var department = _mapper.Map<Department>(departmentViewModel);
                _unitOfWork.Departments.Add(department);

                await _unitOfWork.CommitAsync();

                dataResponse = BaseDataResponse<EditDepartmentViewModel>.Success(_mapper.Map<EditDepartmentViewModel>(department));
            }

            return dataResponse;
        }

        private async Task<bool> CheckDepartmentNameUnique(string name)
        {
            return await _unitOfWork.Departments.AnyAsync(p => p.Name.ToLower() == name.ToLower());
        }

        public async Task<BaseDataResponse<EditDepartmentViewModel>> UpdateAsync(EditDepartmentViewModel model)
        {
            BaseDataResponse<EditDepartmentViewModel> dataResponse;

            var departmentExists = await _unitOfWork.Departments.AnyAsync(p => p.Id == model.Id);

            if (departmentExists)
            {
                var checkNameUnique = await _unitOfWork.Departments
                    .GetDbSet()
                    .AnyAsync(d => d.Id != model.Id && d.Name.ToLower() == model.Name.ToLower());
                if (checkNameUnique)
                {
                    dataResponse = BaseDataResponse<EditDepartmentViewModel>.Fail(model, new ErrorModel("We have already this department"));
                }
                else
                {
                    var department = _mapper.Map<Department>(model);

                    _unitOfWork.Departments.Update(department);

                    await _unitOfWork.CommitAsync();

                    dataResponse = BaseDataResponse<EditDepartmentViewModel>.Success(_mapper.Map<EditDepartmentViewModel>(department));
                }
            }
            else
            {
                dataResponse = BaseDataResponse<EditDepartmentViewModel>.NotFound(model);
            }

            return dataResponse;
        }
    }
}