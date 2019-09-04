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
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Handbook.NewCategories;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class NewsCategoryService : INewsCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NewsCategoryService(IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private IQueryable<NewsCategory> GetQueryWithUser()
        {
            return _unitOfWork.NewsCategories.GetDbSet()
                .Include(p => p.CreatedUser)
                .ThenInclude(p => p.Employee);
        }

        private IQueryable<NewsCategory> FilterQuery(FilterHandbookViewModel filterHandbook, IQueryable<NewsCategory> query)
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

        public async Task<BaseDataResponse<ResponseNewsCategoryViewModel>> GetByIdAsync(Guid id)
        {
            var existsNewsCategory = await  GetQueryWithUser().FirstOrDefaultAsync(p => p.Id == id);
            if (existsNewsCategory == null)
            {
                return BaseDataResponse<ResponseNewsCategoryViewModel>.NotFound(null);
            }

            var existsNewsCategoryLog = await _unitOfWork.NewsCategoryLogs.GetDbSet()
                .Include(p => p.CreatedUser)
                .ThenInclude(p => p.Employee)
                .OrderByDescending( p => p.CreatedAt)
                .FirstOrDefaultAsync(p => p.NewsCategoryId == existsNewsCategory.Id);
            if (existsNewsCategoryLog != null)
            {
                existsNewsCategory.CreatedUser = existsNewsCategoryLog.CreatedUser;
                existsNewsCategory.CreatedAt = existsNewsCategoryLog.CreatedAt;
            }

            return BaseDataResponse<ResponseNewsCategoryViewModel>.Success(_mapper.Map<ResponseNewsCategoryViewModel>(existsNewsCategory));
        }

        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllSelectListItemAsync()
        {
            var categorySelectList = await _unitOfWork.NewsCategories.GetDbSet()
                .OrderByDescending( p => p.CreatedAt)
                .Where(p => p.IsActive)
                .Select(p =>
                    new SelectListItemViewModel()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        ItemsCount = _unitOfWork.News.GetDbSet().Count(m => m.NewsCategoryId == p.Id)
                    }).ToListAsync();

            var vm = _mapper.Map<IEnumerable<SelectListItemViewModel>>(categorySelectList);
            
            return BaseDataResponse<IEnumerable<SelectListItemViewModel>>.Success(vm);
        }

        public async Task<BaseDataResponse<PagedList<ResponseNewsCategoryViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule)
        {
            var query = GetQueryWithUser();

            query = FilterQuery(filterHandbook, query);

            var queryVm = query.ProjectTo<ResponseNewsCategoryViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<ResponseNewsCategoryViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<ResponseNewsCategoryViewModel>> CreateAsync(RequestNewsCategoryViewModel requestNewsCategoryViewModel)
        {

            var existsNewsCategory =
                _unitOfWork.NewsCategories.Any(e => e.Name.ToLower() == requestNewsCategoryViewModel.Name.ToLower());
            if (existsNewsCategory)
                return BaseDataResponse<ResponseNewsCategoryViewModel>.Fail(
                    _mapper.Map<ResponseNewsCategoryViewModel>(requestNewsCategoryViewModel),
                    new ErrorModel(ErrorCode.NameMustBeUnique));

            var newsCategory = _mapper.Map<NewsCategory>(requestNewsCategoryViewModel);
            _unitOfWork.NewsCategories.Add(newsCategory);

            await _unitOfWork.CommitAsync();

            return await GetByIdAsync(newsCategory.Id);
        }

        public async Task<BaseDataResponse<ResponseNewsCategoryViewModel>> EditAsync(RequestNewsCategoryViewModel responseNewsCategoryViewModel)
        {
            var  newsCategoryExists = await _unitOfWork.NewsCategories.GetDbSet().FirstOrDefaultAsync( p => p.Id == responseNewsCategoryViewModel.Id);
            if (newsCategoryExists == null)
                return BaseDataResponse<ResponseNewsCategoryViewModel>.NotFound(null);

            var checkNameUnique = await _unitOfWork.NewsCategories
                    .GetDbSet()
                    .AnyAsync(n => n.Id != responseNewsCategoryViewModel.Id 
                                   && n.Name.ToLower() == responseNewsCategoryViewModel.Name.ToLower());
            if (checkNameUnique)
                return BaseDataResponse<ResponseNewsCategoryViewModel>.Fail(
                    _mapper.Map<ResponseNewsCategoryViewModel>(responseNewsCategoryViewModel),
                    new ErrorModel(ErrorCode.NameMustBeUnique));
            

            var newsCategoryLog = _mapper.Map<NewsCategoryLog>(newsCategoryExists);
            _unitOfWork.NewsCategoryLogs.Add(newsCategoryLog);

            var newsCategory = _mapper.Map<RequestNewsCategoryViewModel, NewsCategory>(responseNewsCategoryViewModel, newsCategoryExists);
            _unitOfWork.NewsCategories.Update(newsCategory);

            await _unitOfWork.CommitAsync();

            return await GetByIdAsync(newsCategory.Id);
        }

        public async Task<BaseDataResponse<PagedList<NewsCategoryLogViewModel>>> GetAllLogAsync(Guid id, PagingOptions pagingOptions, SortRule sortRule)
        {
            var query = _unitOfWork.NewsCategoryLogs.GetDbSet().Where(p => p.NewsCategoryId == id);

            var queryVm = query.ProjectTo<NewsCategoryLogViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<NewsCategoryLogViewModel>>.Success(pagedList);
        }
    }
}
