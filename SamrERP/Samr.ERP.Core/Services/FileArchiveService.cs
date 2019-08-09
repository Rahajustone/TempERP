using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.FileArchive;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Position;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.Infrastructure.Extensions;

namespace Samr.ERP.Core.Services
{
    public class FileArchiveService : IFileArchiveService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public FileArchiveService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

        private IQueryable<FileArchive> GetQuery()
        {
            return _unitOfWork.FileArchives.GetDbSet()
                .OrderByDescending( p => p.CreatedAt)
                .Include(f => f.FileArchiveCategory);
        }

        private IQueryable<FileArchive> GetQueryWithUser()
        {
            return GetQuery().Include(u => u.CreatedUser);
        }

        private IQueryable<FileArchive> GetQueryWithInclude()
        {
            return GetQuery()
                .OrderByDescending( p => p.CreatedAt)
                .Include(p => p.CreatedUser)
                .ThenInclude(p => p.Employee)
                .ThenInclude(p => p.Position);
        }

        private IQueryable<FileArchive> FilterQuery(FilterFileArchiveViewModel filterFileArchiveViewModel, IQueryable<FileArchive> query)
        {
            if (filterFileArchiveViewModel.FromDate != null)
            {
                var fromDate = Convert.ToDateTime(filterFileArchiveViewModel.FromDate);
                query = query.Where(p => p.CreatedAt.Date >= fromDate);
            }

            if (filterFileArchiveViewModel.ToDate != null)
            {
                var toDate = Convert.ToDateTime(filterFileArchiveViewModel.ToDate);
                query = query.Where(p => p.CreatedAt.Date <= toDate);
            }

            if (filterFileArchiveViewModel.ShortDescription != null)
            {
                var filterShortDesc = filterFileArchiveViewModel.ShortDescription.ToLower();
                query = query.Where(f => EF.Functions.Like(f.Title.ToLower(), "%" + filterShortDesc + "%"));
            }

            if (filterFileArchiveViewModel.OnlyActive)
            {
                query = query.Where(n => n.IsActive);
            }

            if (!filterFileArchiveViewModel.CategoryId.IsEmpty())
            {
                query = query.Where(f => f.FileCategoryId == filterFileArchiveViewModel.CategoryId);
            }

            return query;
        }

        public async Task<BaseDataResponse<EditFileArchiveViewModel>> GetByIdAsync(Guid id)
        {
            var existsFileArchive = await GetQueryWithInclude().FirstOrDefaultAsync(u => u.Id == id);
            if (existsFileArchive == null)
            {
                return BaseDataResponse<EditFileArchiveViewModel>.NotFound(null); 
            }

            var vm = _mapper.Map<EditFileArchiveViewModel>(existsFileArchive);
            vm.FilePath = FileService.GetFileArchivePath(vm.FilePath);

            return BaseDataResponse<EditFileArchiveViewModel>.Success(vm);
        }

        public async Task<BaseDataResponse<PagedList<EditFileArchiveViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterFileArchiveViewModel filterFileArchiveViewModel, SortRule sortRule)
        {
            var query = GetQueryWithInclude().Include(c => c.FileArchiveCategory).AsQueryable();

            query = FilterQuery(filterFileArchiveViewModel, query);

            var queryVm = query.ProjectTo<EditFileArchiveViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Title);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            foreach (var allFileArchiveViewModel in pagedList.Items)
            {
                allFileArchiveViewModel.FilePath = FileService.GetFileArchivePath(allFileArchiveViewModel.FilePath);
            }

            return BaseDataResponse<PagedList<EditFileArchiveViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<EditFileArchiveViewModel>> CreateAsync(EditFileArchiveViewModel editFileArchiveViewModel)
        {
            BaseDataResponse<EditFileArchiveViewModel> response;

            var existsFileArchive = await _unitOfWork.FileArchives.GetDbSet()
                .FirstOrDefaultAsync(p => p.Title.ToLower() == editFileArchiveViewModel.Title.ToLower());
            if (existsFileArchive != null)
            {
                response = BaseDataResponse<EditFileArchiveViewModel>.Fail(editFileArchiveViewModel, new ErrorModel("Entity  was found with the same name!"));
            }
            else
            {
                var fileArchive = _mapper.Map<FileArchive>(editFileArchiveViewModel);

                if (editFileArchiveViewModel.File != null)
                {
                    fileArchive.FilePath = await _fileService.SaveFile(FileService.FileArchiveFolderPath, editFileArchiveViewModel.File);
                }

                _unitOfWork.FileArchives.Add(fileArchive);

                await _unitOfWork.CommitAsync();

                response = BaseDataResponse<EditFileArchiveViewModel>.Success(_mapper.Map<EditFileArchiveViewModel>(fileArchive));
            }

            return response;
        }

        public async Task<BaseDataResponse<EditFileArchiveViewModel>> EditAsync(EditFileArchiveViewModel editFileArchiveViewModel)
        {
            BaseDataResponse<EditFileArchiveViewModel> response;

            var existFileArchive = await GetQuery().FirstOrDefaultAsync(p => p.Id == editFileArchiveViewModel.Id);
            if (existFileArchive == null)
            {
                response = BaseDataResponse<EditFileArchiveViewModel>.NotFound(editFileArchiveViewModel);
            }
            else
            {
                var checkNameUnique = await GetQuery()
                    .AnyAsync(d => d.Id != editFileArchiveViewModel.Id && d.Title.ToLower() == editFileArchiveViewModel.Title.ToLower());
                if (checkNameUnique)
                {
                    response = BaseDataResponse<EditFileArchiveViewModel>.Fail(editFileArchiveViewModel,
                        new ErrorModel("We have already this entity."));
                }
                else
                {
                    var fileArchive = _mapper.Map<EditFileArchiveViewModel, FileArchive>(editFileArchiveViewModel, existFileArchive);

                    if (editFileArchiveViewModel.File != null)
                    {
                        fileArchive.FilePath = await _fileService.SaveFile(FileService.FileArchiveFolderPath, editFileArchiveViewModel.File);
                    }

                    _unitOfWork.FileArchives.Update(fileArchive);

                    await _unitOfWork.CommitAsync();

                    response = BaseDataResponse<EditFileArchiveViewModel>.Success(_mapper.Map<EditFileArchiveViewModel>(fileArchive));
                }
            }

            return response;
        }
    }
}
