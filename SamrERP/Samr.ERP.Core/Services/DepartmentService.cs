using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Department;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHandbookService _handbookService;

        public DepartmentService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IHandbookService handbookService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _handbookService = handbookService;
        }

        private IQueryable<Department> GetQueryWithUser()
        {
            return _unitOfWork.Departments.GetDbSet().Include(p => p.CreatedUser);
        }

        private IQueryable<Department> FilterQuery(FilterHandbookViewModel filterHandbook, IQueryable<Department> query)
        {
            if (filterHandbook.Name != null)
            {
                query = query.Where(n => EF.Functions.Like(n.Name, "%" + filterHandbook.Name + "%"));
            }

            if (filterHandbook.OnlyActive)
            {
                query = query.Where(n => n.IsActive);
            }

            return query;
        }

        public async Task<BaseDataResponse<EditDepartmentViewModel>> GetByIdAsync(Guid id)
        {
            var department = await GetQueryWithUser().FirstOrDefaultAsync(p => p.Id == id);

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

        public async Task<BaseDataResponse<PagedList<DepartmentViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule)
        {
            var query = GetQueryWithUser();

            query = FilterQuery(filterHandbook, query);

            var queryVm = query.ProjectTo<DepartmentViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<DepartmentViewModel>>.Success(pagedList);
        }

        private async Task<bool> CheckDepartmentNameUnique(string name)
        {
            return await _unitOfWork.Departments.AnyAsync(p => p.Name.ToLower() == name.ToLower());
        }

        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllSelectListItemAsync()
        {
            var departments = await GetQueryWithUser()
                .Where(e => e.IsActive)
                .ToListAsync();

            return BaseDataResponse<IEnumerable<SelectListItemViewModel>>.Success(_mapper.Map<IEnumerable<SelectListItemViewModel>>(departments));
        }

        public async Task<BaseDataResponse<EditDepartmentViewModel>> CreateAsync(EditDepartmentViewModel editDepartmentViewModel)
        {
            BaseDataResponse<EditDepartmentViewModel> dataResponse;

            var departmentExists = await CheckDepartmentNameUnique(editDepartmentViewModel.Name);
            if (departmentExists)
            {
                dataResponse = BaseDataResponse<EditDepartmentViewModel>.Fail(editDepartmentViewModel, new ErrorModel("Already this model in database."));
            }
            else
            {
                var department = _mapper.Map<Department>(editDepartmentViewModel);
                _unitOfWork.Departments.Add(department);

                var handbookExists = await _handbookService.ChangeStatus("Department", department.CreatedUserId);
                if (handbookExists)
                {
                    await _unitOfWork.CommitAsync();

                    dataResponse = BaseDataResponse<EditDepartmentViewModel>.Success(_mapper.Map<EditDepartmentViewModel>(department));
                }
                else
                {
                    dataResponse = BaseDataResponse<EditDepartmentViewModel>.Fail(editDepartmentViewModel, new ErrorModel("Not found handbook."));
                }
            }

            return dataResponse;
        }

        public async Task<BaseDataResponse<EditDepartmentViewModel>> UpdateAsync(EditDepartmentViewModel editDepartmentViewModel)
        {
            BaseDataResponse<EditDepartmentViewModel> dataResponse;

            var departmentExists = await _unitOfWork.Departments.GetDbSet()
                .FirstOrDefaultAsync(p => p.Id == editDepartmentViewModel.Id);

            if (departmentExists != null)
            {
                var checkNameUnique = await _unitOfWork.Departments
                    .GetDbSet()
                    .AnyAsync(d => d.Id != editDepartmentViewModel.Id && d.Name.ToLower() == editDepartmentViewModel.Name.ToLower());
                if (checkNameUnique)
                {
                    dataResponse = BaseDataResponse<EditDepartmentViewModel>.Fail(editDepartmentViewModel, new ErrorModel("We have already this department"));
                }
                else
                {
                    var department = _mapper.Map<EditDepartmentViewModel, Department>(editDepartmentViewModel, departmentExists);

                    _unitOfWork.Departments.Update(department);

                    var handbookExists = await _handbookService.ChangeStatus("Department", department.CreatedUserId);
                    if (handbookExists)
                    {
                        await _unitOfWork.CommitAsync();

                        dataResponse = BaseDataResponse<EditDepartmentViewModel>.Success(_mapper.Map<EditDepartmentViewModel>(department));
                    }
                    else
                    {
                        dataResponse = BaseDataResponse<EditDepartmentViewModel>.Fail(editDepartmentViewModel, new ErrorModel("Not found handbook."));
                    }
                }
            }
            else
            {
                dataResponse = BaseDataResponse<EditDepartmentViewModel>.NotFound(editDepartmentViewModel);
            }

            return dataResponse;
        }
    }
}