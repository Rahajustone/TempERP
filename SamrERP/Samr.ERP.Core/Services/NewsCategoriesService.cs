using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.News.Categories;
using Samr.ERP.Core.ViewModels.Position;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class NewsCategoriesService : INewsCategoriesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NewsCategoriesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BaseDataResponse<NewsCategoriesViewModel>> GetByIdAsync(Guid id)
        {
            var newsCategory = await _unitOfWork.NewsCategories.GetDbSet()
                .Include(p => p.CreatedUser)
                .FirstOrDefaultAsync(p => p.Id == id);

            BaseDataResponse<NewsCategoriesViewModel> dataResponse;

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

        public async Task<BaseDataResponse<IEnumerable<NewsCategoriesViewModel>>> GetAllAsync()
        {
            var newsCategories = await _unitOfWork.NewsCategories.GetDbSet().Include(p => p.CreatedUser).ToListAsync();
            var vm = _mapper.Map<IEnumerable<NewsCategoriesViewModel>>(newsCategories);

            var response = BaseDataResponse<IEnumerable<NewsCategoriesViewModel>>.Success(vm);

            return response;
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

                await _unitOfWork.CommitAsync();

                response = BaseDataResponse<NewsCategoriesViewModel>.Success(_mapper.Map<NewsCategoriesViewModel>(newsCategory));
            }

            return response;
        }

        public async Task<BaseDataResponse<NewsCategoriesViewModel>> UpdateAsync(NewsCategoriesViewModel newsCategoriesViewModel)
        {
            BaseDataResponse<NewsCategoriesViewModel> dataResponse;

            var  newsCategoryExists = await _unitOfWork.NewsCategories.ExistsAsync(newsCategoriesViewModel.Id);
            if (newsCategoryExists)
            {
                var newsCategory = _mapper.Map<NewsCategory>(newsCategoriesViewModel);

                _unitOfWork.NewsCategories.Update(newsCategory);

                await _unitOfWork.CommitAsync();

                dataResponse = BaseDataResponse<NewsCategoriesViewModel>.Success(_mapper.Map<NewsCategoriesViewModel>(newsCategory));
            }
            else
            {
                dataResponse = BaseDataResponse<NewsCategoriesViewModel>.NotFound(newsCategoriesViewModel);
            }

            return dataResponse;
        }
    }
}
