﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.AutoMapper.AutoMapperProfiles;
using Samr.ERP.Core.Enums;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Staff;
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
        private readonly IHistoryService<DepartmentLog, Department> _historyService;

        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper, IHistoryService<DepartmentLog, Department> historyService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _historyService = historyService;
        }

        private IQueryable<Department> GetQueryWithUser()
        {
            return _unitOfWork.Departments.GetDbSet()
                .Include(p => p.CreatedUser)
                .ThenInclude( p => p.Employee)
                .OrderByDescending(p => p.CreatedAt);
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

        public async Task<BaseDataResponse<ResponseDepartmentViewModel>> GetByIdAsync(Guid id)
        {
            var existsDepartment = await GetQueryWithUser()
                .FirstOrDefaultAsync(p => p.Id == id);
            
            if (existsDepartment == null)
                return  BaseDataResponse<ResponseDepartmentViewModel>.NotFound(null);

            var existsDepartmentLog = await _unitOfWork.DepartmentLogs.GetDbSet()
                .OrderByDescending( p => p.CreatedAt)
                .Include(p => p.CreatedUser)
                .ThenInclude(p => p.Employee)
                .FirstOrDefaultAsync(a => a.DepartmentId == existsDepartment.Id);

            if (existsDepartmentLog != null)
            {
                existsDepartment.CreatedUser = existsDepartmentLog.CreatedUser;
                existsDepartment.CreatedAt = existsDepartmentLog.CreatedAt;
            }

             return BaseDataResponse<ResponseDepartmentViewModel>.Success(_mapper.Map<ResponseDepartmentViewModel>(existsDepartment));
        }

        public async Task<BaseDataResponse<PagedList<EditDepartmentViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule)
        {
            var query = GetQueryWithUser();

            // TODO
                //.Select( p => new DepartmentListViewModel
                //{
                //    Department =  p,
                //    DepartmentLog =  _unitOfWork.DepartmentLogs.GetDbSet().Take(1).FirstOrDefault( j => j.DepartmentId == p.Id)
                //})
                //.AsQueryable()
                //;

            query = FilterQuery(filterHandbook, query);

            var queryVm = query.ProjectTo<EditDepartmentViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<EditDepartmentViewModel>>.Success(pagedList);
        }

        private async Task<bool> CheckDepartmentNameUnique(string name)
        {
            return await _unitOfWork.Departments.AnyAsync(p => p.Name.ToLower() == name.ToLower());
        }

        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllSelectListItemAsync()
        {
            var departments = await GetQueryWithUser()
                .Where(e => e.IsActive)
                .OrderBy(p=>p.Name)
                .ToListAsync();

            return BaseDataResponse<IEnumerable<SelectListItemViewModel>>.Success(_mapper.Map<IEnumerable<SelectListItemViewModel>>(departments));
        }

        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllSelectListItemWithPositionAsync()
        {
            var departmentsWithPosition = GetQueryWithUser()
                .Include( p => p.Positions)
                .Where( a => a.Positions.Any())
                .Where(e => e.IsActive)
                .ToListAsync();

            return BaseDataResponse<IEnumerable<SelectListItemViewModel>>.Success(
                _mapper.Map<IEnumerable<SelectListItemViewModel>>(departmentsWithPosition));
        }

        public async Task<BaseDataResponse<ResponseDepartmentViewModel>> CreateAsync(RequestDepartmentViewModel requestDepartmentViewModel)
        {

            var departmentExists = await CheckDepartmentNameUnique(requestDepartmentViewModel.Name);
            if (departmentExists)
                return BaseDataResponse<ResponseDepartmentViewModel>.NotFound(
                    _mapper.Map<ResponseDepartmentViewModel>(requestDepartmentViewModel),
                    new ErrorModel(ErrorCode.NameMustBeUnique));

            var department = _mapper.Map<Department>(requestDepartmentViewModel);
            _unitOfWork.Departments.Add(department);

            await _unitOfWork.CommitAsync();

            return await GetByIdAsync(department.Id);
        }

        public async Task<BaseDataResponse<ResponseDepartmentViewModel>> EditAsync(RequestDepartmentViewModel requestDepartmentViewModel)
        {

            var departmentExists = await _unitOfWork.Departments.GetDbSet()
                .FirstOrDefaultAsync(p => p.Id == requestDepartmentViewModel.Id);
            if (departmentExists == null)
                return BaseDataResponse<ResponseDepartmentViewModel>.NotFound(null);

            var checkNameUnique = await _unitOfWork.Departments
                    .GetDbSet()
                    .AnyAsync(d =>
                        d.Id != requestDepartmentViewModel.Id &&
                        d.Name.ToLower() == requestDepartmentViewModel.Name.ToLower());
            if (checkNameUnique)
                return BaseDataResponse<ResponseDepartmentViewModel>.Fail(
                    _mapper.Map<ResponseDepartmentViewModel>(requestDepartmentViewModel),
                    new ErrorModel(ErrorCode.NameMustBeUnique));

            var departmentLog = _mapper.Map<DepartmentLog>(departmentExists);
            _unitOfWork.DepartmentLogs.Add(departmentLog);

            var department = _mapper.Map<RequestDepartmentViewModel, Department>(requestDepartmentViewModel, departmentExists);
            _unitOfWork.Departments.Update(department);

            await _unitOfWork.CommitAsync();

            return await GetByIdAsync(department.Id);
        }

        public async Task<BaseDataResponse<PagedList<DepartmentLogViewModel>>> GetAllLogAsync(Guid id, PagingOptions pagingOptions, SortRule sortRule)
        {
            var query = _unitOfWork.DepartmentLogs.GetDbSet()
                .Where(d => d.DepartmentId == id)
                .Include(p => p.CreatedUser)
                .ThenInclude(p => p.Employee)
                .OrderByDescending( p => p.CreatedAt);

            var queryVm = query.ProjectTo<DepartmentLogViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<DepartmentLogViewModel>>.Success(pagedList);
        }
    }
}