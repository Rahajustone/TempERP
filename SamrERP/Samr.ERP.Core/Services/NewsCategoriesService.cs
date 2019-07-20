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
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Handbook.NewCategories;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class NewsCategoriesService : INewsCategoriesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHandbookService _handbookService;

        public NewsCategoriesService(IUnitOfWork unitOfWork, 
            IMapper mapper,
            IHandbookService handbookService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _handbookService = handbookService;
        }

        private IQueryable<NewsCategory> GetQueryWithUser()
        {
            return _unitOfWork.NewsCategories.GetDbSet().Include(p => p.CreatedUser);
        }

        private IQueryable<NewsCategory> FilterQuery(FilterHandbookViewModel filterHandbook, IQueryable<NewsCategory> query)
        {
            if (filterHandbook.Name != null)
            {
                query = query.Where(n => EF.Functions.Like(n.Name, "%" + filterHandbook.Name + "%"));
            }

            if (filterHandbook.IsActive)
            {
                query = query.Where(n => n.IsActive);
            }

            return query;
        }

        public async Task<BaseDataResponse<NewsCategoriesViewModel>> GetByIdAsync(Guid id)
        {
            BaseDataResponse<NewsCategoriesViewModel> dataResponse;

            var newsCategory = await  GetQueryWithUser().FirstOrDefaultAsync(p => p.Id == id);

            if (newsCategory == null)
            {
                dataResponse = BaseDataResponse<NewsCategoriesViewModel>.NotFound(null);
            }
            else
            {
                dataResponse = BaseDataResponse<NewsCategoriesViewModel>.Success(_mapper.Map<NewsCategoriesViewModel>(newsCategory));
            }

            return dataResponse;
        }

        public async Task<BaseDataResponse<PagedList<NewsCategoriesViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule)
        {
            var query = GetQueryWithUser();

            query = FilterQuery(filterHandbook, query);

            var queryVm = query.ProjectTo<NewsCategoriesViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<NewsCategoriesViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<NewsCategoriesViewModel>> CreateAsync(NewsCategoriesViewModel newsCategoriesViewModel)
        {
            BaseDataResponse<NewsCategoriesViewModel> response;

            var exists =
                _unitOfWork.NewsCategories.Any(e => e.Name.ToLower() == newsCategoriesViewModel.Name.ToLower());

            if (exists)
            {
                response = BaseDataResponse<NewsCategoriesViewModel>.Fail(newsCategoriesViewModel, new ErrorModel("Name already exist!"));
            }
            else
            {
                var newsCategory = _mapper.Map<NewsCategory>(newsCategoriesViewModel);
                _unitOfWork.NewsCategories.Add(newsCategory);

                var handbookExists = await _handbookService.ChangeStatus("Department", newsCategory.CreatedUserId );
                if (handbookExists)
                {
                    await _unitOfWork.CommitAsync();

                    response = BaseDataResponse<NewsCategoriesViewModel>.Success(_mapper.Map<NewsCategoriesViewModel>(newsCategory));
                }
                else
                {
                    response = BaseDataResponse<NewsCategoriesViewModel>.Fail(newsCategoriesViewModel, new ErrorModel("Not found handbook."));
                }
            }

            return response;
        }

        public async Task<BaseDataResponse<NewsCategoriesViewModel>> Editsync(NewsCategoriesViewModel newsCategoriesViewModel)
        {
            BaseDataResponse<NewsCategoriesViewModel> dataResponse;

            var  newsCategoryExists = await _unitOfWork.NewsCategories.GetDbSet().FirstOrDefaultAsync( p => p.Id == newsCategoriesViewModel.Id);
            if (newsCategoryExists != null)
            {
                var checkNameUnique = await _unitOfWork.NewsCategories
                    .GetDbSet()
                    .AnyAsync(n => n.Id != newsCategoriesViewModel.Id 
                                   && n.Name.ToLower() == newsCategoriesViewModel.Name.ToLower());
                if (checkNameUnique)
                {
                    dataResponse = BaseDataResponse<NewsCategoriesViewModel>.Fail(newsCategoriesViewModel, new ErrorModel("We have already this news categories with this name"));
                }
                else
                {
                    var newsCategory = _mapper.Map<NewsCategoriesViewModel, NewsCategory>(newsCategoriesViewModel, newsCategoryExists);

                    _unitOfWork.NewsCategories.Update(newsCategory);

                    var handbookExists = await _handbookService.ChangeStatus("Department", newsCategory.CreatedUserId );
                    if (handbookExists)
                    {
                        await _unitOfWork.CommitAsync();

                        dataResponse = BaseDataResponse<NewsCategoriesViewModel>.Success(_mapper.Map<NewsCategoriesViewModel>(newsCategory));
                    }
                    else
                    {
                        dataResponse = BaseDataResponse<NewsCategoriesViewModel>.Fail(newsCategoriesViewModel, new ErrorModel("Not found handbook."));
                    }
                }
            }
            else
            {
                dataResponse = BaseDataResponse<NewsCategoriesViewModel>.NotFound(newsCategoriesViewModel);
            }

            return dataResponse;
        }
    }
}
