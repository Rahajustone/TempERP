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
using Samr.ERP.Core.ViewModels.Handbook.UsefulLinkCategory;
using Samr.ERP.Core.ViewModels.UsefulLink;
using Samr.ERP.Core.ViewModels.UsefulLink.UsefulLinkCategory;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class UsefulLinkService : IUsefulLinkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsefulLinkService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private IQueryable<UsefulLink> GetQuery()
        {
            return _unitOfWork.UsefulLinks.GetDbSet()
                .OrderByDescending(p => p.CreatedAt)
                .Include(p => p.CreatedUser)
                .Include(c => c.UsefulLinkCategory);
        }

        private IQueryable<UsefulLink> GetQueryWithInclude()
        {
            return GetQuery()
                .Include(p => p.CreatedUser)
                .ThenInclude(p => p.Employee)
                .ThenInclude(p => p.Position);
        }

        private IQueryable<UsefulLink> GetQueryFilter(FilterUsefulLinkViewModel filterUsefulLinkViewModel, IQueryable<UsefulLink> query)
        {
            if (filterUsefulLinkViewModel.FromDate != null)
            {
                var fromDate = Convert.ToDateTime(filterUsefulLinkViewModel.FromDate + " 00:00");
                query = query.Where(p => p.CreatedAt >= fromDate);
            }

            if (filterUsefulLinkViewModel.ToDate != null)
            {
                var toDate = Convert.ToDateTime(filterUsefulLinkViewModel.ToDate + " 23:59");
                query = query.Where(p => p.CreatedAt <= toDate);
            }

            if (filterUsefulLinkViewModel.Title != null)
                query = query.Where(p => EF.Functions.Like(p.Title.ToLower(), "%" + filterUsefulLinkViewModel.Title.ToLower() + "%"));

            if (filterUsefulLinkViewModel.CategoryId != Guid.Empty)
                query = query.Where(p => p.UsefulLinkCategoryId == filterUsefulLinkViewModel.CategoryId);

            if (filterUsefulLinkViewModel.OnlyActive)
            {
                query = query.Where(n => n.IsActive);
            }

            return query;
        }

        public async Task<BaseDataResponse<UsefulLinkViewModel>> GetByIdAsync(Guid id)
        {
            var existsUsefulLink = await GetQueryWithInclude().FirstOrDefaultAsync(u => u.Id == id);
            if (existsUsefulLink == null)
            {
                return BaseDataResponse<UsefulLinkViewModel>.NotFound(null);
            }

            return BaseDataResponse<UsefulLinkViewModel>.Success(_mapper.Map<UsefulLinkViewModel>(existsUsefulLink));
        }

        public async Task<BaseDataResponse<PagedList<UsefulLinkViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterUsefulLinkViewModel filterUsefulLinkViewModel)
        {
            var query = GetQueryWithInclude();
            query = GetQueryFilter(filterUsefulLinkViewModel, query);

            var pagedList = await query.ToMappedPagedListAsync<UsefulLink, UsefulLinkViewModel>(pagingOptions);

            return BaseDataResponse<PagedList<UsefulLinkViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<UsefulLinkViewModel>> CreateAsync(UsefulLinkViewModel editUsefulLinkViewModel)
        {
            BaseDataResponse<UsefulLinkViewModel> response;

            var existsUsefulLink = await GetQuery()
                .FirstOrDefaultAsync(p => p.Title.ToLower() == editUsefulLinkViewModel.Title.ToLower());
            if (existsUsefulLink != null)
            {
                response = BaseDataResponse<UsefulLinkViewModel>.Fail(editUsefulLinkViewModel, new ErrorModel("Entity  was found with the same name!"));
            }
            else
            {
                var usefulLink = _mapper.Map<UsefulLink>(editUsefulLinkViewModel);

                _unitOfWork.UsefulLinks.Add(usefulLink);

                await _unitOfWork.CommitAsync();

                response = BaseDataResponse<UsefulLinkViewModel>.Success(_mapper.Map<UsefulLinkViewModel>(usefulLink));
            }

            return response;
        }

        public async Task<BaseDataResponse<UsefulLinkViewModel>> EditAsync(UsefulLinkViewModel editUsefulLinkViewModel)
        {
            BaseDataResponse<UsefulLinkViewModel> response;

            var existUsefulLink = await GetQuery().FirstOrDefaultAsync(p => p.Id == editUsefulLinkViewModel.Id);
            if (existUsefulLink == null)
            {
                response = BaseDataResponse<UsefulLinkViewModel>.NotFound(editUsefulLinkViewModel);
            }
            else
            {
                var checkNameUnique = await GetQuery()
                    .AnyAsync(d => d.Id != editUsefulLinkViewModel.Id && d.Title.ToLower() == editUsefulLinkViewModel.Title.ToLower());
                if (checkNameUnique)
                {
                    response = BaseDataResponse<UsefulLinkViewModel>.Fail(editUsefulLinkViewModel,
                        new ErrorModel("We have already this useful link category"));
                }
                else
                {
                    var usefulLink = _mapper.Map<UsefulLinkViewModel, UsefulLink>(editUsefulLinkViewModel, existUsefulLink);

                    _unitOfWork.UsefulLinks.Update(usefulLink);

                    await _unitOfWork.CommitAsync();

                    response = BaseDataResponse<UsefulLinkViewModel>.Success(_mapper.Map<UsefulLinkViewModel>(usefulLink));
                }
            }

            return response;
        }

        public async Task<BaseDataResponse<PagedList<UsefulLinkCategoryLogViewModel>>> GetAllLogAsync(Guid id, PagingOptions pagingOptions, SortRule sortRule)
        {
            var query = _unitOfWork.UserLockReasonLogs.GetDbSet()
                .Where(p => p.UserLockReasonId == id)
                .OrderByDescending( p => p.CreatedAt);

            var queryVm = query.ProjectTo<UsefulLinkCategoryLogViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<UsefulLinkCategoryLogViewModel>>.Success(pagedList);
        }
    }
}
