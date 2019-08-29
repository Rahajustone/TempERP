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
using Samr.ERP.Core.ViewModels.Handbook.UserLockReason;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class UserLockReasonService : IUserLockReasonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserLockReasonService(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private IQueryable<UserLockReason> GetQuery()
        {
            return _unitOfWork.UserLockReasons.GetDbSet().OrderByDescending( p => p.CreatedAt);
        }

        private IQueryable<UserLockReason> GetQueryWithUser()
        {
            return GetQuery().Include(u => u.CreatedUser).ThenInclude( p => p.Employee);
        }

        private IQueryable<UserLockReason> FilterQuery(FilterHandbookViewModel filterHandbook, IQueryable<UserLockReason> query)
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

        public async Task<BaseDataResponse<ResponseUserLockReasonViewModel>> GetByIdAsync(Guid id)
        {
            var existsUserLockReason = await GetQueryWithUser().FirstOrDefaultAsync(u => u.Id == id);
            if (existsUserLockReason == null)
            {
                return BaseDataResponse<ResponseUserLockReasonViewModel>.NotFound(null);
            }

            var existsUserLockReasonLog = await _unitOfWork.UserLockReasonLogs.GetDbSet()
                .Include(p => p.CreatedUser)
                .ThenInclude(p => p.Employee)
                .FirstOrDefaultAsync(p => p.UserLockReasonId == existsUserLockReason.Id);

            if (existsUserLockReasonLog != null)
            {
                existsUserLockReason.CreatedUser = existsUserLockReasonLog.CreatedUser;
                existsUserLockReason.CreatedAt = existsUserLockReasonLog.CreatedAt;
            }

            return BaseDataResponse<ResponseUserLockReasonViewModel>.Success(_mapper.Map<ResponseUserLockReasonViewModel>(existsUserLockReason));
        }

        public async Task<BaseDataResponse<PagedList<ResponseUserLockReasonViewModel>>> GetAllAsync(
            PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule)
        {
            var query = GetQueryWithUser();

            query = FilterQuery(filterHandbook, query);

            var queryVm = query.ProjectTo<ResponseUserLockReasonViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<ResponseUserLockReasonViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllListItemsAsync()
        {
            var listItem = await GetQueryWithUser().Where(u => u.IsActive).ToListAsync();

            return BaseDataResponse<IEnumerable<SelectListItemViewModel>>.Success(_mapper.Map<IEnumerable<SelectListItemViewModel>>(listItem));
        }

        public async Task<BaseDataResponse<ResponseUserLockReasonViewModel>> CreateAsync(RequestUserLockReasonViewModel userLockReasonViewModel)
        {
            var existsUserLockReason = _unitOfWork.UserLockReasons.Any(p => p.Name.ToLower() == userLockReasonViewModel.Name.ToLower());
            if (existsUserLockReason)
            {
                return BaseDataResponse<ResponseUserLockReasonViewModel>.Fail(
                    _mapper.Map<ResponseUserLockReasonViewModel>(userLockReasonViewModel),
                    new ErrorModel(ErrorCode.NameMustBeUnique));
            }

            var userLockReason = _mapper.Map<UserLockReason>(userLockReasonViewModel);
            _unitOfWork.UserLockReasons.Add(userLockReason);

            await _unitOfWork.CommitAsync();

            return await GetByIdAsync(userLockReason.Id);
        }

        public async Task<BaseDataResponse<ResponseUserLockReasonViewModel>> EditAsync(RequestUserLockReasonViewModel userLockReasonViewModel)
        {
            var userLockReasonExists = await GetQuery().FirstOrDefaultAsync( u => u.Id == userLockReasonViewModel.Id);
            if (userLockReasonExists == null)
            {
                return BaseDataResponse<ResponseUserLockReasonViewModel>.NotFound(null);
            }

            var checkNameUnique = await GetQuery().AnyAsync(u => u.Id != userLockReasonViewModel.Id 
                                   && u.Name.ToLower() == userLockReasonViewModel.Name.ToLower());
            if (checkNameUnique)
                return BaseDataResponse<ResponseUserLockReasonViewModel>.NotFound(
                    _mapper.Map<ResponseUserLockReasonViewModel>(userLockReasonViewModel),
                    new ErrorModel(ErrorCode.NameMustBeUnique));
            
            var userLockReasonLog = _mapper.Map<UserLockReasonLog>(userLockReasonExists);
            _unitOfWork.UserLockReasonLogs.Add(userLockReasonLog);

            var userLockReason = _mapper.Map<RequestUserLockReasonViewModel, UserLockReason>(userLockReasonViewModel, userLockReasonExists);
            _unitOfWork.UserLockReasons.Update(userLockReason);

            await _unitOfWork.CommitAsync();

            return await GetByIdAsync(userLockReason.Id);
        }

        public async Task<BaseDataResponse<PagedList<UserLockReasonLogViewModel>>> GetAllLogAsync(Guid id, PagingOptions pagingOptions, SortRule sortRule)
        {
            var query = _unitOfWork.UserLockReasonLogs.GetDbSet()
                .Where(p => p.UserLockReasonId == id)
                .OrderByDescending( p => p.CreatedAt);

            var queryVm = query.ProjectTo<UserLockReasonLogViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<UserLockReasonLogViewModel>>.Success(pagedList);
        }
    }
}
