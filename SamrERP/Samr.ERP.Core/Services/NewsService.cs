using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.News;
using Samr.ERP.Core.ViewModels.News.Categories;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NewsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseDataResponse<EditNewsViewModel>> GetByIdAsync(Guid id)
        {
            BaseDataResponse<EditNewsViewModel> response;

            var existsNews = await _unitOfWork.News.GetDbSet()
                .Include(n => n.NewsCategory)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (existsNews == null)
            {
                response = BaseDataResponse<EditNewsViewModel>.NotFound(null);
            }
            else
            {
                response = BaseDataResponse<EditNewsViewModel>.Success(_mapper.Map<EditNewsViewModel>(existsNews));
            }

            return response;
        }

        public async Task<BaseDataResponse<IEnumerable<EditNewsViewModel>>> GetAllAsync()
        {
            var news = await _unitOfWork.News.GetDbSet().Include(u => u.NewsCategory).ToListAsync();
            var vm = _mapper.Map<IEnumerable<EditNewsViewModel>>(news);

            var response = BaseDataResponse<IEnumerable<EditNewsViewModel>>.Success(vm);

            return response;
        }

        public async Task<BaseDataResponse<EditNewsViewModel>> CreateAsync(EditNewsViewModel newsViewModel)
        {
            BaseDataResponse<EditNewsViewModel> response;

            var newsExists = _unitOfWork.News.Any(u => u.ShortDescription == newsViewModel.ShortDescription);
            if (newsExists)
            {
                response = BaseDataResponse<EditNewsViewModel>.Fail(newsViewModel, new ErrorModel("ShortDescription must not be unique"));
            }
            else
            {
                var news = _mapper.Map<News>(newsViewModel);
                _unitOfWork.News.Add(news);

                await _unitOfWork.CommitAsync();

                // TODO
                news = await _unitOfWork.News.GetDbSet()
                    .Include(p => p.NewsCategory)
                    .FirstOrDefaultAsync(p => p.Id == news.Id);

                response = BaseDataResponse<EditNewsViewModel>.Success(_mapper.Map<EditNewsViewModel>(news));
            }

            return response;
        }

        public async Task<BaseDataResponse<EditNewsViewModel>> UpdateAsync(EditNewsViewModel newsViewModel)
        {
            BaseDataResponse<EditNewsViewModel> dataResponse;

            var newsExists = await _unitOfWork.News.ExistsAsync(newsViewModel.Id);
            if (newsExists)
            {
                var news = _mapper.Map<News>(newsViewModel);

                _unitOfWork.News.Update(news);

                await _unitOfWork.CommitAsync();

                dataResponse = BaseDataResponse<EditNewsViewModel>.Success(_mapper.Map<EditNewsViewModel>(news));
            }
            else
            {
                dataResponse = BaseDataResponse<EditNewsViewModel>.NotFound(newsViewModel);
            }

            return dataResponse;
        }
    }
}
