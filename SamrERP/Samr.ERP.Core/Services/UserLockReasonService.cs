﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            return _unitOfWork.UserLockReasons.GetDbSet();
        }

        private IQueryable<UserLockReason> GetQueryWithUser()
        {
            return GetQuery().Include(u => u.CreatedUser);
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

        public async Task<BaseDataResponse<UserLockReasonViewModel>> GetByIdAsync(Guid id)
        {
            BaseDataResponse<UserLockReasonViewModel> response;

            var existsUserLockReason = await GetQueryWithUser().FirstOrDefaultAsync(u => u.Id == id);
            if (existsUserLockReason == null)
            {
                response = BaseDataResponse<UserLockReasonViewModel>.NotFound(null, new ErrorModel("No such record!"));
            }
            else
            {
                response = BaseDataResponse<UserLockReasonViewModel>.Success(_mapper.Map<UserLockReasonViewModel>(existsUserLockReason));
            }

            return response;
        }

        public async Task<BaseDataResponse<PagedList<UserLockReasonViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule)
        {
            var query = GetQueryWithUser();

            query = FilterQuery(filterHandbook, query);

            var queryVm = query.ProjectTo<UserLockReasonViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<UserLockReasonViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllListItemsAsync()
        {
            var listItem = await GetQueryWithUser().Where(u => u.IsActive).ToListAsync();

            return BaseDataResponse<IEnumerable<SelectListItemViewModel>>.Success(_mapper.Map<IEnumerable<SelectListItemViewModel>>(listItem));
        }

        public async Task<BaseDataResponse<UserLockReasonViewModel>> CreateAsync(UserLockReasonViewModel userLockReasonViewModel)
        {
            BaseDataResponse<UserLockReasonViewModel> response;

            var existsUserLockReason = _unitOfWork.UserLockReasons.Any(p => p.Name.ToLower() == userLockReasonViewModel.Name.ToLower());
            if (existsUserLockReason)
            {
                response = BaseDataResponse<UserLockReasonViewModel>.Fail(userLockReasonViewModel, new ErrorModel("UserLockReason already exist."));
            }
            else
            {
                var userLockReason = _mapper.Map<UserLockReason>(userLockReasonViewModel);

                _unitOfWork.UserLockReasons.Add(userLockReason);

                await _unitOfWork.CommitAsync();

                response = BaseDataResponse<UserLockReasonViewModel>.Success(_mapper.Map<UserLockReasonViewModel>(userLockReason));
            }

            return response;
        }

        public async Task<BaseDataResponse<UserLockReasonViewModel>> EditAsync(UserLockReasonViewModel userLockReasonViewModel)
        {
            BaseDataResponse<UserLockReasonViewModel> dataResponse;

            var userLockReasonExists = await GetQuery().FirstOrDefaultAsync( u => u.Id == userLockReasonViewModel.Id);
            if (userLockReasonExists != null)
            {
                var checkNameUnique = await GetQuery().AnyAsync(u => u.Id != userLockReasonViewModel.Id 
                                   && u.Name.ToLower() == userLockReasonViewModel.Name.ToLower());
                if (checkNameUnique)
                {
                    dataResponse = BaseDataResponse<UserLockReasonViewModel>.NotFound(userLockReasonViewModel, new ErrorModel("Duplicate name of lock reason!"));
                }
                else
                {
                    var userLockReasonLog = _mapper.Map<UserLockReasonLog>(userLockReasonExists);
                    _unitOfWork.UserLockReasonLogs.Add(userLockReasonLog);

                    var userLockReason = _mapper.Map<UserLockReasonViewModel, UserLockReason>(userLockReasonViewModel, userLockReasonExists);
                    _unitOfWork.UserLockReasons.Update(userLockReason);

                    await _unitOfWork.CommitAsync();

                    dataResponse = BaseDataResponse<UserLockReasonViewModel>.Success(_mapper.Map<UserLockReasonViewModel>(userLockReason));
                }
            }
            else
            {
                dataResponse = BaseDataResponse<UserLockReasonViewModel>.NotFound(userLockReasonViewModel);
            }

            return dataResponse;
        }

        public async Task<BaseDataResponse<PagedList<UserLockReasonLogViewModel>>> GetAllLogAsync(Guid id, PagingOptions pagingOptions, SortRule sortRule)
        {
            var query = _unitOfWork.UserLockReasonLogs.GetDbSet().Where(p => p.UserLockReasonId == id);

            var queryVm = query.ProjectTo<UserLockReasonLogViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<UserLockReasonLogViewModel>>.Success(pagedList);
        }
    }
}
