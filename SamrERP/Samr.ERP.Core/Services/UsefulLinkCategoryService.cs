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
using Samr.ERP.Core.Staff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Department;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Handbook.UsefulLinkCategory;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class UsefulLinkCategoryService : IUsefulLinkCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsefulLinkCategoryService(IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        private IQueryable<UsefulLinkCategory> GetQuery()
        {
            return _unitOfWork.UsefulLinkCategories.GetDbSet().OrderByDescending( p => p.CreatedAt);
        }

        private IQueryable<UsefulLinkCategory> GetQueryWithUser()
        {
            return GetQuery().Include(p => p.CreatedUser).ThenInclude(p => p.Employee);
        }

        private IQueryable<UsefulLinkCategory> FilterQuery(FilterHandbookViewModel filterHandbook, IQueryable<UsefulLinkCategory> query)
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

        public async Task<BaseDataResponse<ResponseUsefulLinkCategoryViewModel>> GetByIdAsync(Guid id)
        {
            var existUsefulLinkCategory = await GetQuery().FirstOrDefaultAsync(u => u.Id == id);
            if (existUsefulLinkCategory == null)
            {
                return BaseDataResponse<ResponseUsefulLinkCategoryViewModel>.NotFound(null);
            }

            var existUsefulLinkCategoryLog = await _unitOfWork.UsefulLinkCategoryLogs.GetDbSet()
                .Include(p => p.CreatedUser)
                .ThenInclude(p => p.Employee)
                .OrderByDescending( p => p.CreatedAt)
                .OrderByDescending( p => p.CreatedAt)
                .FirstOrDefaultAsync(u => u.UsefulLinkCategoryId == existUsefulLinkCategory.Id);

            if (existUsefulLinkCategoryLog != null)
            {
                existUsefulLinkCategory.CreatedUser = existUsefulLinkCategoryLog.CreatedUser;
                existUsefulLinkCategory.CreatedAt = existUsefulLinkCategoryLog.CreatedAt;
            }

            return BaseDataResponse<ResponseUsefulLinkCategoryViewModel>.Success(
                _mapper.Map<ResponseUsefulLinkCategoryViewModel>(existUsefulLinkCategory));
        }

        public async Task<BaseDataResponse<PagedList<ResponseUsefulLinkCategoryViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule)
        {
            var query = GetQueryWithUser();

            query = FilterQuery(filterHandbook, query);

            var queryVm = query.ProjectTo<ResponseUsefulLinkCategoryViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<ResponseUsefulLinkCategoryViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllSelectListItemAsync()
        {
            var categorySelectList = await _unitOfWork.UsefulLinkCategories
                .GetDbSet()
                .Where(p => p.IsActive)
                .OrderByDescending( p => p.CreatedAt)
                .Select(p =>
                    new SelectListItemViewModel()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        ItemsCount = _unitOfWork.UsefulLinks.GetDbSet().Where( _ => _.IsActive).Count(m => m.UsefulLinkCategoryId == p.Id)
                    }).ToListAsync();

            return BaseDataResponse<IEnumerable<SelectListItemViewModel>>.Success(categorySelectList);
        }

        public async Task<BaseDataResponse<ResponseUsefulLinkCategoryViewModel>> CreateAsync(RequestUsefulLinkCategoryViewModel requestUsefulLinkCategoryViewModel)
        {
            var existsUsefulLinkCategories = await GetQuery().FirstOrDefaultAsync(p => p.Name.ToLower() == requestUsefulLinkCategoryViewModel.Name.ToLower());
            if (existsUsefulLinkCategories != null)
            {
                return BaseDataResponse<ResponseUsefulLinkCategoryViewModel>.Fail(
                    _mapper.Map<ResponseUsefulLinkCategoryViewModel>(requestUsefulLinkCategoryViewModel),
                    new ErrorModel(ErrorCode.NameMustBeUnique));
            }

            var usefulLinkCategory = _mapper.Map<UsefulLinkCategory>(requestUsefulLinkCategoryViewModel);
            _unitOfWork.UsefulLinkCategories.Add(usefulLinkCategory);

            await _unitOfWork.CommitAsync();

            return await GetByIdAsync(usefulLinkCategory.Id);
        }

        public async Task<BaseDataResponse<ResponseUsefulLinkCategoryViewModel>> EditAsync(RequestUsefulLinkCategoryViewModel requestUsefulLinkCategoryViewModel)
        {
            var existUsefulLinkCategory = await GetQuery().FirstOrDefaultAsync(p => p.Id == requestUsefulLinkCategoryViewModel.Id);
            if (existUsefulLinkCategory == null)
                return BaseDataResponse<ResponseUsefulLinkCategoryViewModel>.NotFound(null);

            var checkNameUnique = await GetQuery()
                .AnyAsync(d => d.Id != requestUsefulLinkCategoryViewModel.Id && d.Name.ToLower() == requestUsefulLinkCategoryViewModel.Name.ToLower());
            if (checkNameUnique)
                return BaseDataResponse<ResponseUsefulLinkCategoryViewModel>.Fail(
                    _mapper.Map<ResponseUsefulLinkCategoryViewModel>(requestUsefulLinkCategoryViewModel),
                    new ErrorModel(ErrorCode.NameMustBeUnique));

            var usefulLinkCategoryLog = _mapper.Map<UsefulLinkCategoryLog>(existUsefulLinkCategory);
            _unitOfWork.UsefulLinkCategoryLogs.Add(usefulLinkCategoryLog);

            var usefulLinkCategory = _mapper.Map<RequestUsefulLinkCategoryViewModel, UsefulLinkCategory>(requestUsefulLinkCategoryViewModel, existUsefulLinkCategory);
            _unitOfWork.UsefulLinkCategories.Update(usefulLinkCategory);

            await _unitOfWork.CommitAsync();

            return await GetByIdAsync(usefulLinkCategory.Id);
        }

        public async Task<BaseDataResponse<PagedList<UsefulLinkCategoryLogViewModel>>> GetAllLogAsync(Guid id, PagingOptions pagingOptions, SortRule sortRule)
        {
            var query = _unitOfWork.UsefulLinkCategoryLogs.GetDbSet()
                .Where(p => p.UsefulLinkCategoryId == id)
                .OrderByDescending( p => p.CreatedAt);

            var queryVm = query.ProjectTo<UsefulLinkCategoryLogViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<UsefulLinkCategoryLogViewModel>>.Success(pagedList);
        }
    }
}
