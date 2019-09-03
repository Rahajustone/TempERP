using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Enums;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Handbook.EmployeeLockReason;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class EmployeeLockReasonService : IEmployeeLockReasonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeLockReasonService(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private IQueryable<EmployeeLockReason> GetQuery()
        {
            return _unitOfWork.EmployeeLockReasons.GetDbSet()
                .OrderByDescending( p => p.CreatedAt);
        }

        private IQueryable<EmployeeLockReason> GetQueryWithUser()
        {
            return GetQuery().Include(p => p.CreatedUser).ThenInclude(p => p.Employee );
        }

        private bool EmployeeLockReasonExists(RequestEmployeeLockReasonViewModel employeeLockReasonViewModel)
        {
            return _unitOfWork.EmployeeLockReasons.Any(p => p.Name.ToLower() == employeeLockReasonViewModel.Name.ToLower());
        }

        private IQueryable<EmployeeLockReason> FilterQuery(FilterHandbookViewModel filterHandbook, IQueryable<EmployeeLockReason> query)
        {
            if (filterHandbook.Name != null)
            {
                query = query.Where(n => EF.Functions.Like(n.Name.ToLower(), "%" + filterHandbook.Name.ToLower() + "%"));
            }

            if (filterHandbook.OnlyActive)
            {
                query = query.Where(n => n.IsActive);
            }

            return query;
        }

        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllListItemAsync()
        {
            var listItem = await GetQuery().Where(e => e.IsActive).ToListAsync();

            var vm = _mapper.Map<IEnumerable<SelectListItemViewModel>>(listItem);

            return BaseDataResponse<IEnumerable<SelectListItemViewModel>>.Success(vm);
        }

        public async Task<BaseDataResponse<ResponseEmployeeLockReasonViewModel>> GetByIdAsync(Guid id)
        {
            var existEmployeeLockReason = await GetQueryWithUser().FirstOrDefaultAsync(p => p.Id == id);
            
            if (existEmployeeLockReason == null)
            {
                return BaseDataResponse<ResponseEmployeeLockReasonViewModel>.NotFound(null);
            }

            var existEmployeeLockReasonLog = await _unitOfWork.EmployeeLockReasonLog.GetDbSet()
                .Include(p => p.CreatedUser)
                .ThenInclude(p => p.Employee)
                .FirstOrDefaultAsync(p => p.EmployeeLockReasonId == existEmployeeLockReason.Id);

            if (existEmployeeLockReasonLog != null)
            {
                existEmployeeLockReason.CreatedUser = existEmployeeLockReasonLog.CreatedUser;
                existEmployeeLockReason.CreatedAt = existEmployeeLockReasonLog.CreatedAt;
            }

            return BaseDataResponse<ResponseEmployeeLockReasonViewModel>.Success(
                _mapper.Map<ResponseEmployeeLockReasonViewModel>(existEmployeeLockReason));
        }

        public async Task<BaseDataResponse<PagedList<ResponseEmployeeLockReasonViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule)
        {
            var query = GetQueryWithUser();

            query = FilterQuery(filterHandbook, query);

            var queryVm = query.ProjectTo<ResponseEmployeeLockReasonViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<ResponseEmployeeLockReasonViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<ResponseEmployeeLockReasonViewModel>> CreateAsync(RequestEmployeeLockReasonViewModel employeeLockReasonViewModel)
        {
            var employeeLockReasonExists = EmployeeLockReasonExists(employeeLockReasonViewModel);
            if (employeeLockReasonExists)
            {
                return BaseDataResponse<ResponseEmployeeLockReasonViewModel>.Fail(
                    _mapper.Map<ResponseEmployeeLockReasonViewModel>(employeeLockReasonViewModel),
                    new ErrorModel(ErrorCode.NameMustBeUnique));
            }

            var employeeLockReason = _mapper.Map<EmployeeLockReason>(employeeLockReasonViewModel);
            _unitOfWork.EmployeeLockReasons.Add(employeeLockReason);

            await _unitOfWork.CommitAsync();

            return await GetByIdAsync(employeeLockReason.Id);
        }

        public async Task<BaseDataResponse<ResponseEmployeeLockReasonViewModel>> EditAsync(RequestEmployeeLockReasonViewModel employeeLockReasonViewModel)
        {
            var employeeLockReasonExists = await GetQuery()
                .FirstOrDefaultAsync(e => e.Id == employeeLockReasonViewModel.Id);

            if (employeeLockReasonExists == null)
            {
                return BaseDataResponse<ResponseEmployeeLockReasonViewModel>.NotFound(null);
            }

            var checkNameUnique = await _unitOfWork.EmployeeLockReasons
                .GetDbSet()
                .AnyAsync(e => e.Id != employeeLockReasonViewModel.Id && e.Name == employeeLockReasonViewModel.Name);
            if (checkNameUnique)
            {
                return BaseDataResponse<ResponseEmployeeLockReasonViewModel>.NotFound(
                    _mapper.Map<ResponseEmployeeLockReasonViewModel>(employeeLockReasonViewModel),
                    new ErrorModel(ErrorCode.NameMustBeUnique));
            }

            var employeeLockReasonLog = _mapper.Map<EmployeeLockReasonLog>(employeeLockReasonExists);
            _unitOfWork.EmployeeLockReasonLog.Add(employeeLockReasonLog);

            var employeeLockReason = _mapper.Map<RequestEmployeeLockReasonViewModel, EmployeeLockReason>(employeeLockReasonViewModel, employeeLockReasonExists);
            _unitOfWork.EmployeeLockReasons.Update(employeeLockReason);
  
            await _unitOfWork.CommitAsync();

            return await GetByIdAsync(employeeLockReasonExists.Id);
        }

        public async Task<BaseDataResponse<PagedList<EmployeeLockReasonLogViewModel>>> GetAllLogAsync(Guid id, PagingOptions pagingOptions, SortRule sortRule)
        {
            var query = _unitOfWork.EmployeeLockReasonLog.GetDbSet().Where(p => p.EmployeeLockReasonId == id);

            var queryVm = query.ProjectTo<EmployeeLockReasonLogViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<EmployeeLockReasonLogViewModel>>.Success(pagedList);
        }
    }
}
