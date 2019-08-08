using System;
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
            return _unitOfWork.NewsCategories.GetDbSet().Include(p => p.CreatedUser);
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

        public async Task<BaseDataResponse<NewsCategoryViewModel>> GetByIdAsync(Guid id)
        {
            BaseDataResponse<NewsCategoryViewModel> dataResponse;

            var newsCategory = await  GetQueryWithUser().FirstOrDefaultAsync(p => p.Id == id);

            if (newsCategory == null)
            {
                dataResponse = BaseDataResponse<NewsCategoryViewModel>.NotFound(null);
            }
            else
            {
                dataResponse = BaseDataResponse<NewsCategoryViewModel>.Success(_mapper.Map<NewsCategoryViewModel>(newsCategory));
            }

            return dataResponse;
        }

        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllSelectListItemAsync()
        {
            var categorySelectList = await _unitOfWork.News
                    .GetDbSet()
                    .Include(p => p.NewsCategory)
                    .GroupBy(p => p.NewsCategoryId)
                    .Select(p =>
                        new SelectListItemViewModel()
                        {
                            Id = p.Key,
                            Name = p.First().NewsCategory.Name,
                            ItemsCount = p.Count()
                        }).ToListAsync();
                
            var vm = _mapper.Map<IEnumerable<SelectListItemViewModel>>(categorySelectList);
            
            return BaseDataResponse<IEnumerable<SelectListItemViewModel>>.Success(vm);
        }

        public async Task<BaseDataResponse<PagedList<NewsCategoryViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule)
        {
            var query = GetQueryWithUser();

            query = FilterQuery(filterHandbook, query);

            var queryVm = query.ProjectTo<NewsCategoryViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<NewsCategoryViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<NewsCategoryViewModel>> CreateAsync(NewsCategoryViewModel newsCategoryViewModel)
        {
            BaseDataResponse<NewsCategoryViewModel> response;

            var exists =
                _unitOfWork.NewsCategories.Any(e => e.Name.ToLower() == newsCategoryViewModel.Name.ToLower());

            if (exists)
            {
                response = BaseDataResponse<NewsCategoryViewModel>.Fail(newsCategoryViewModel, new ErrorModel("Name already exist!"));
            }
            else
            {
                var newsCategory = _mapper.Map<NewsCategory>(newsCategoryViewModel);
                _unitOfWork.NewsCategories.Add(newsCategory);

                await _unitOfWork.CommitAsync();

                response = BaseDataResponse<NewsCategoryViewModel>.Success(_mapper.Map<NewsCategoryViewModel>(newsCategory));
            }

            return response;
        }

        public async Task<BaseDataResponse<NewsCategoryViewModel>> EditAsync(NewsCategoryViewModel newsCategoryViewModel)
        {
            BaseDataResponse<NewsCategoryViewModel> dataResponse;

            var  newsCategoryExists = await _unitOfWork.NewsCategories.GetDbSet().FirstOrDefaultAsync( p => p.Id == newsCategoryViewModel.Id);
            if (newsCategoryExists != null)
            {
                var checkNameUnique = await _unitOfWork.NewsCategories
                    .GetDbSet()
                    .AnyAsync(n => n.Id != newsCategoryViewModel.Id 
                                   && n.Name.ToLower() == newsCategoryViewModel.Name.ToLower());
                if (checkNameUnique)
                {
                    dataResponse = BaseDataResponse<NewsCategoryViewModel>.Fail(newsCategoryViewModel, new ErrorModel("We have already this news categories with this name"));
                }
                else
                {
                    var newsCategoryLog = _mapper.Map<NewsCategoryLog>(newsCategoryExists);
                    _unitOfWork.NewsCategoryLogs.Add(newsCategoryLog);

                    var newsCategory = _mapper.Map<NewsCategoryViewModel, NewsCategory>(newsCategoryViewModel, newsCategoryExists);
                    _unitOfWork.NewsCategories.Update(newsCategory);

                    await _unitOfWork.CommitAsync();

                    dataResponse = BaseDataResponse<NewsCategoryViewModel>.Success(_mapper.Map<NewsCategoryViewModel>(newsCategory));
                }
            }
            else
            {
                dataResponse = BaseDataResponse<NewsCategoryViewModel>.NotFound(newsCategoryViewModel);
            }

            return dataResponse;
        }
    }
}
