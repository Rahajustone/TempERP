using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Org.BouncyCastle.Math.EC;
using Remotion.Linq.Clauses;
using Samr.ERP.Core.Auth;
using Samr.ERP.Core.Enums;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Employee;
using Samr.ERP.Core.ViewModels.News;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.Infrastructure.Providers;

namespace Samr.ERP.Core.Services
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly UserManager<User> _userManager;
        private readonly UserProvider _userProvider;

        public NewsService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileService fileService,
            UserManager<User> userManager,
            UserProvider userProvider)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
            _userManager = userManager;
            _userProvider = userProvider;
        }

        private IQueryable<News> GetQuery()
        {
            return _unitOfWork.News.GetDbSet()
                .OrderByDescending(p => p.PublishAt)
                .Include(n => n.NewsCategory);
        }

        private IQueryable<News> GetQueryWithInclude()
        {
            return GetQuery()
                .Include(p => p.CreatedUser)
                .ThenInclude(p => p.Employee)
                .ThenInclude(p => p.Position);
        }

        private static IQueryable<News> GetFilterQuery(FilterNewsViewModel filterNewViewModel, IQueryable<News> query)
        {
            if (filterNewViewModel.FromDate != null)
            {
                var fromDate = Convert.ToDateTime(filterNewViewModel.FromDate);
                query = query.Where(p => p.CreatedAt.Date >= fromDate);
            }

            if (filterNewViewModel.ToDate != null)
            {
                var toDate = Convert.ToDateTime(filterNewViewModel.ToDate);
                query = query.Where(p => p.CreatedAt.Date <= toDate);
            }

            if (filterNewViewModel.Title != null)
                query = query.Where(p => EF.Functions.Like(p.Title.ToLower(), "%" + filterNewViewModel.Title.ToLower() + "%"));

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

            var existsNews = await GetQueryWithInclude().FirstOrDefaultAsync(p => p.Id == id);

            if (existsNews == null)
            {
                response = BaseDataResponse<EditNewsViewModel>.NotFound(null);
            }
            else
            {
                var vm = _mapper.Map<EditNewsViewModel>(existsNews);

                response = BaseDataResponse<EditNewsViewModel>.Success(vm);
            }

            return response;
        }

        public async Task<BaseDataResponse<PagedList<EditNewsViewModel>>> GetAllAsync(PagingOptions  pagingOptions, FilterNewsViewModel filterNewViewModel)
        {
            var query = GetQueryWithInclude();

            //// TODO
            //var result = await _userManager.IsInRoleAsync(_userProvider.CurrentUser, Roles.NewsCreate);
            //if (!result)
            //{
            //    query = query.Where(p => p.PublishAt <= DateTime.Now);
            //}

            query = GetFilterQuery(filterNewViewModel, query);

            var queryVm = query.ProjectTo<EditNewsViewModel>();

            var pagedList = await queryVm.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<EditNewsViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<EditNewsViewModel>> CreateAsync(EditNewsViewModel newsViewModel)
        {
            BaseDataResponse<EditNewsViewModel> response;

            var newsExists = _unitOfWork.News.Any(u => u.ShortDescription == newsViewModel.ShortDescription);
            if (newsExists)
            {
                response = BaseDataResponse<EditNewsViewModel>.Fail(newsViewModel, new ErrorModel(ErrorCode.NameMustBeUnique));
            }
            else
            {
                var news = _mapper.Map<News>(newsViewModel);
                if ((int)news.PublishAt.TimeOfDay.TotalSeconds == 0)
                {
                   news.PublishAt = news.PublishAt.Add(DateTime.Now.TimeOfDay);
                }

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
                    dataResponse = BaseDataResponse<EditNewsViewModel>.Fail(newsViewModel,new ErrorModel(ErrorCode.NameMustBeUnique));
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
