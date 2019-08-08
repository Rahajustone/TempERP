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
using Samr.ERP.Core.ViewModels.Handbook.FileArchiveCategory;
using Samr.ERP.Core.ViewModels.Position;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;
using FileArchiveCategoryLog = Samr.ERP.Infrastructure.Entities.FileArchiveCategoryLog;

namespace Samr.ERP.Core.Services
{
    public class FileArchiveCategoryService : IFileArchiveCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FileArchiveCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private IQueryable<FileArchiveCategory> GetQuery()
        {
            return _unitOfWork.FileArchiveCategories.GetDbSet().Include(p => p.CreatedUser);
        }

        private IQueryable<FileArchiveCategory> FilterQuery(FilterHandbookViewModel filterHandbook, IQueryable<FileArchiveCategory> query)
        {
            if (filterHandbook.Name != null)
            {
                var filter = filterHandbook.Name.ToLower();
                query = query.Where(n => EF.Functions.Like(n.Name.ToLower(), "%" + filter +"%"));
            }

            if (filterHandbook.OnlyActive)
            {
                query = query.Where(n => n.IsActive);
            }

            return query;
        }

        public async Task<BaseDataResponse<EditFileArchiveCategoryViewModel>> GetByIdAsync(Guid id)
        {
            var existsFileCategory = await GetQuery().FirstOrDefaultAsync(u => u.Id == id);
            if (existsFileCategory == null)
            {
                return BaseDataResponse<EditFileArchiveCategoryViewModel>.NotFound(null);
            }

            return BaseDataResponse<EditFileArchiveCategoryViewModel>.Success(_mapper.Map<EditFileArchiveCategoryViewModel>(existsFileCategory));
        }

        public async Task<BaseDataResponse<PagedList<EditFileArchiveCategoryViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule)
        {
            var query = GetQuery();

            query = FilterQuery(filterHandbook, query);

            var queryVm = query.ProjectTo<EditFileArchiveCategoryViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<EditFileArchiveCategoryViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllSelectListItemAsync()
        {
            var categorySelectList = await _unitOfWork.FileArchives
                .GetDbSet()
                .OrderByDescending(p => p.CreatedAt)
                .Include(p => p.FileArchiveCategory)
                .GroupBy(p => p.FileCategoryId)
                .Select(p =>
                    new SelectListItemViewModel()
                    {
                        Id = p.Key,
                        Name = p.First().FileArchiveCategory.Name,
                        ItemsCount = p.Count()
                    }).ToListAsync();

            var vm = _mapper.Map<IEnumerable<SelectListItemViewModel>>(categorySelectList);

            return BaseDataResponse<IEnumerable<SelectListItemViewModel>>.Success(vm);
        }

        public async Task<BaseDataResponse<EditFileArchiveCategoryViewModel>> CreateAsync(EditFileArchiveCategoryViewModel fileArchiveCategoryViewModel)
        {
            BaseDataResponse<EditFileArchiveCategoryViewModel> dataResponse;

            var fileCategoryExists = _unitOfWork.FileArchiveCategories.Any(p => p.Name.ToLower() == fileArchiveCategoryViewModel.Name.ToLower());
            if (fileCategoryExists)
            {
                dataResponse = BaseDataResponse<EditFileArchiveCategoryViewModel>.Fail(fileArchiveCategoryViewModel, new ErrorModel("Already this model in database."));
            }
            else
            {
                var fileCategory = _mapper.Map<FileArchiveCategory>(fileArchiveCategoryViewModel);
                _unitOfWork.FileArchiveCategories.Add(fileCategory);

                await _unitOfWork.CommitAsync();

                dataResponse = BaseDataResponse<EditFileArchiveCategoryViewModel>.Success(_mapper.Map<EditFileArchiveCategoryViewModel>(fileCategory));
            }

            return dataResponse;
        }

        public async Task<BaseDataResponse<EditFileArchiveCategoryViewModel>> EditAsync(EditFileArchiveCategoryViewModel editFileArchiveCategoryViewModel)
        {
            BaseDataResponse<EditFileArchiveCategoryViewModel> dataResponse;

            var existsFileCategory = await GetQuery().FirstOrDefaultAsync(u => u.Id == editFileArchiveCategoryViewModel.Id);
            if (existsFileCategory != null)
            {
                var checkNameUnique = await GetQuery().AnyAsync(u => u.Id != editFileArchiveCategoryViewModel.Id
                                                                     && u.Name.ToLower() == editFileArchiveCategoryViewModel.Name.ToLower());
                if (checkNameUnique)
                {
                    dataResponse = BaseDataResponse<EditFileArchiveCategoryViewModel>.NotFound(editFileArchiveCategoryViewModel, new ErrorModel("Duplicate name of file category!"));
                }
                else
                {
                    var fileCategoryLog = _mapper.Map<FileArchiveCategoryLog>(existsFileCategory);
                    _unitOfWork.FileArchiveCategoryLogs.Add(fileCategoryLog);

                    var fileCategory = _mapper.Map<EditFileArchiveCategoryViewModel, FileArchiveCategory>(editFileArchiveCategoryViewModel, existsFileCategory);
                    _unitOfWork.FileArchiveCategories.Update(fileCategory);

                    await _unitOfWork.CommitAsync();

                    dataResponse = BaseDataResponse<EditFileArchiveCategoryViewModel>.Success(_mapper.Map<EditFileArchiveCategoryViewModel>(fileCategory));
                }
            }
            else
            {
                dataResponse = BaseDataResponse<EditFileArchiveCategoryViewModel>.NotFound(editFileArchiveCategoryViewModel);
            }

            return dataResponse;
        }

        public async Task<BaseDataResponse<PagedList<FileArchiveCategoryLogViewModel>>> GetAllLogAsync(Guid id, PagingOptions pagingOptions, SortRule sortRule)
        {
            var query = _unitOfWork.UsefulLinkCategoryLogs.GetDbSet().Where(p => p.UsefulLinkCategoryId == id);

            var queryVm = query.ProjectTo<FileArchiveCategoryLogViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<FileArchiveCategoryLogViewModel>>.Success(pagedList);
        }
    }
}

