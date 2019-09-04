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
            return _unitOfWork.FileArchiveCategories.GetDbSet()
                .Include(p => p.CreatedUser)
                .ThenInclude(p => p.Employee)
                .OrderByDescending(p => p.CreatedAt);
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

        public async Task<BaseDataResponse<ResponseFileArchiveCategoryViewModel>> GetByIdAsync(Guid id)
        {
            var existsFileCategory = await GetQuery().FirstOrDefaultAsync(u => u.Id == id);
            if (existsFileCategory == null)
                return BaseDataResponse<ResponseFileArchiveCategoryViewModel>.NotFound(null);

            var existsFileCategoryLog = await _unitOfWork.FileArchiveCategoryLogs.GetDbSet()
                .Include(p => p.CreatedUser)
                .ThenInclude(p => p.Employee)
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync(p => p.FileCategoryId == existsFileCategory.Id);
            if (existsFileCategoryLog != null)
            {
                existsFileCategory.CreatedUser = existsFileCategoryLog.CreatedUser;
                existsFileCategory.CreatedAt = existsFileCategoryLog.CreatedAt;
            }

            return BaseDataResponse<ResponseFileArchiveCategoryViewModel>.Success(
                _mapper.Map<ResponseFileArchiveCategoryViewModel>(existsFileCategory));
        }

        public async Task<BaseDataResponse<PagedList<ResponseFileArchiveCategoryViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule)
        {
            var query = GetQuery();

            query = FilterQuery(filterHandbook, query);

            var queryVm = query.ProjectTo<ResponseFileArchiveCategoryViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<ResponseFileArchiveCategoryViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllSelectListItemAsync()
        {
            var categorySelectList = await GetQuery()
                .Where(p=>p.IsActive)
                .Select(p =>
                    new SelectListItemViewModel()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        ItemsCount = _unitOfWork.FileArchives.GetDbSet().Count(m => m.FileCategoryId == p.Id)
                    })
                .ToListAsync();

            var vm = _mapper.Map<IEnumerable<SelectListItemViewModel>>(categorySelectList);

            return BaseDataResponse<IEnumerable<SelectListItemViewModel>>.Success(vm);
        }

        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetCategoriesWithFileArchiveAllSelectListItemAsync()
        {
            var categorySelectList = await GetQuery()
                .Include(p => p.FileArchives)
                .Where( p => p.FileArchives.Any())
                .Where(p => p.IsActive)
                .Select(p =>
                    new SelectListItemViewModel()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        ItemsCount = p.FileArchives.Count
                    })
                .ToListAsync();

            var vm = _mapper.Map<IEnumerable<SelectListItemViewModel>>(categorySelectList);

            return BaseDataResponse<IEnumerable<SelectListItemViewModel>>.Success(vm);
        }

        public async Task<BaseDataResponse<ResponseFileArchiveCategoryViewModel>> CreateAsync(RequestFileArchiveCategoryViewModel fileArchiveCategoryViewModel)
        {
            var fileCategoryExists = _unitOfWork.FileArchiveCategories.Any(p => p.Name.ToLower() == fileArchiveCategoryViewModel.Name.ToLower());
            if (fileCategoryExists)
                return BaseDataResponse<ResponseFileArchiveCategoryViewModel>.Fail(
                    _mapper.Map<ResponseFileArchiveCategoryViewModel>(fileArchiveCategoryViewModel),
                    new ErrorModel(ErrorCode.NameMustBeUnique));

            var fileCategory = _mapper.Map<FileArchiveCategory>(fileArchiveCategoryViewModel);
            _unitOfWork.FileArchiveCategories.Add(fileCategory);

            await _unitOfWork.CommitAsync();

            return await GetByIdAsync(fileCategory.Id);
        }

        public async Task<BaseDataResponse<ResponseFileArchiveCategoryViewModel>> EditAsync(RequestFileArchiveCategoryViewModel editFileArchiveCategoryViewModel)
        {
            var existsFileCategory = await GetQuery().FirstOrDefaultAsync(u => u.Id == editFileArchiveCategoryViewModel.Id);
            if (existsFileCategory == null)
                return BaseDataResponse<ResponseFileArchiveCategoryViewModel>.NotFound(null);

            var checkNameUnique = await GetQuery().AnyAsync(u => u.Id != editFileArchiveCategoryViewModel.Id
                                                                 && u.Name.ToLower() == editFileArchiveCategoryViewModel.Name.ToLower());
            if (checkNameUnique)
                return BaseDataResponse<ResponseFileArchiveCategoryViewModel>.NotFound(
                    _mapper.Map<ResponseFileArchiveCategoryViewModel>(editFileArchiveCategoryViewModel),
                    new ErrorModel(ErrorCode.NameMustBeUnique));

            var fileCategoryLog = _mapper.Map<FileArchiveCategoryLog>(existsFileCategory);
            _unitOfWork.FileArchiveCategoryLogs.Add(fileCategoryLog);

            var fileCategory = _mapper.Map<RequestFileArchiveCategoryViewModel, FileArchiveCategory>(editFileArchiveCategoryViewModel, existsFileCategory);
            _unitOfWork.FileArchiveCategories.Update(fileCategory);

            await _unitOfWork.CommitAsync();

            return await GetByIdAsync(fileCategory.Id);
        }

        public async Task<BaseDataResponse<PagedList<FileArchiveCategoryLogViewModel>>> GetAllLogAsync(Guid id, PagingOptions pagingOptions, SortRule sortRule)
        {
            var query = _unitOfWork.FileArchiveCategoryLogs.GetDbSet().Where(p => p.FileCategoryId == id).OrderByDescending(p => p.CreatedAt);

            var queryVm = query.ProjectTo<FileArchiveCategoryLogViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<FileArchiveCategoryLogViewModel>>.Success(pagedList);
        }
    }
}

