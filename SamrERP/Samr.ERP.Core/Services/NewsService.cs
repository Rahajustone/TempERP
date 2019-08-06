using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Remotion.Linq.Clauses;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.News;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public NewsService(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

        private IQueryable<News> GetQuery()
        {
            return _unitOfWork.News.GetDbSet()
                .Include(n => n.NewsCategory);
        }

        private static IQueryable<News> GetFilterQuery(FilterNewsViewModel filterNewViewModel, IQueryable<News> query)
        {
            if (filterNewViewModel.FromDate.HasValue)
            {
                query = query.Where(p => p.CreatedAt >= filterNewViewModel.FromDate);
            }

            if (filterNewViewModel.ToDate.HasValue)
            {
                query = query.Where(p => p.CreatedAt <= filterNewViewModel.FromDate);
            }

            if (filterNewViewModel.Title != null)
                query = query.Where(p => EF.Functions.Like(p.Title, "%" + filterNewViewModel.Title + "%"));

            if (filterNewViewModel.CategoryId != Guid.Empty)
                query = query.Where(p => p.NewsCategoryId == filterNewViewModel.CategoryId);

            if (filterNewViewModel.OnlyActive)
            {
                query = query.Where(n => n.IsActive);
            }

            return query;
        }

        public async Task<BaseDataResponse<EditNewsViewModel>> GetByIdAsync(Guid id)
        {
            BaseDataResponse<EditNewsViewModel> response;

            var existsNews = await GetQuery().FirstOrDefaultAsync(p => p.Id == id);
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

        public async Task<BaseDataResponse<PagedList<EditNewsViewModel>>> GetAllAsync(PagingOptions  pagingOptions, FilterNewsViewModel filterNewViewModel)
        {
            var query =  GetQuery();
            query = GetFilterQuery(filterNewViewModel, query);

            var pageList = await query.ToMappedPagedListAsync<News, EditNewsViewModel>(pagingOptions);

            return BaseDataResponse<PagedList<EditNewsViewModel>>.Success(pageList);
        }

        

        public async Task<BaseDataResponse<EditNewsViewModel>> CreateAsync(EditNewsViewModel newsViewModel)
        {
            BaseDataResponse<EditNewsViewModel> response;

            var newsExists = _unitOfWork.News.Any(u => u.ShortDescription == newsViewModel.ShortDescription);
            if (newsExists)
            {
                response = BaseDataResponse<EditNewsViewModel>.Fail(newsViewModel, new ErrorModel("Duplicate Title. Title must be unique"));
            }
            else
            {
                var news = _mapper.Map<News>(newsViewModel);

                if (newsViewModel.ImageFile != null)
                {
                    news.Image = await _fileService.UploadPhoto(FileService.NewsPhotoFolderPath, newsViewModel.ImageFile, true);
                }

                _unitOfWork.News.Add(news);

                await _unitOfWork.CommitAsync();

                response = BaseDataResponse<EditNewsViewModel>.Success(_mapper.Map<EditNewsViewModel>(news));
            }

            return response;
        }

        public async Task<BaseDataResponse<EditNewsViewModel>> EditAsync(EditNewsViewModel newsViewModel)
        {
            BaseDataResponse<EditNewsViewModel> dataResponse;

            var newsExists = await _unitOfWork.News.GetDbSet().FirstOrDefaultAsync( p => p.Id == newsViewModel.Id);
            if (newsExists != null)
            {
                var checkShortDescriptionUnique = await _unitOfWork.News
                    .GetDbSet()
                    .AnyAsync(d => d.Id != newsViewModel.Id
                                   && d.ShortDescription.ToLower() == newsViewModel.ShortDescription.ToLower());
                if (checkShortDescriptionUnique)
                {
                    dataResponse = BaseDataResponse<EditNewsViewModel>.Fail(newsViewModel,
                        new ErrorModel("Duplicate short description field!"));
                }
                else
                {
                    var news = _mapper.Map<EditNewsViewModel, News>(newsViewModel, newsExists);

                    if (newsViewModel.ImageFile != null)
                    {
                        news.Image = await _fileService.UploadPhoto(FileService.NewsPhotoFolderPath, newsViewModel.ImageFile, true);
                    }

                    _unitOfWork.News.Update(news);

                    await _unitOfWork.CommitAsync();

                    dataResponse = BaseDataResponse<EditNewsViewModel>.Success(_mapper.Map<EditNewsViewModel>(news));
                }
            }
            else
            {
                dataResponse = BaseDataResponse<EditNewsViewModel>.NotFound(newsViewModel);
            }

            return dataResponse;
        }
    }
}
