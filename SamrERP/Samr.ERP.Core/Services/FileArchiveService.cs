using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Samr.ERP.Core.Auth;
using Samr.ERP.Core.Enums;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Staff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.FileArchive;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Position;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.Infrastructure.Extensions;
using Samr.ERP.Infrastructure.Providers;

namespace Samr.ERP.Core.Services
{
    public class FileArchiveService : IFileArchiveService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly UserProvider _userProvider;

        public FileArchiveService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileService fileService,
            UserProvider userProvider
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
            _userProvider = userProvider;
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

            if (filterFileArchiveViewModel.Title != null)
            {
                var titleFilter = filterFileArchiveViewModel.Title.ToLower();
                query = query.Where(f => EF.Functions.Like(f.Title.ToLower(), "%" + titleFilter + "%"));
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

        public async Task<BaseDataResponse<GetByIdFileArchiveViewModel>> GetByIdAsync(Guid id)
        {
            var existsFileArchive = await GetQueryWithInclude().FirstOrDefaultAsync(u => u.Id == id);
            if (existsFileArchive == null)
            {
                return BaseDataResponse<GetByIdFileArchiveViewModel>.NotFound(null);
            }

            var vm = _mapper.Map<GetByIdFileArchiveViewModel>(existsFileArchive);
            vm.FilePath = FileService.GetFileArchivePath(vm.FilePath);

            return BaseDataResponse<GetByIdFileArchiveViewModel>.Success(vm);
        }

        public async Task<BaseDataResponse<PagedList<GetListFileArchiveViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterFileArchiveViewModel filterFileArchiveViewModel, SortRule sortRule)
        {
            var query = GetQueryWithInclude().Include(c => c.FileArchiveCategory).AsQueryable();

            query = FilterQuery(filterFileArchiveViewModel, query);

            if (!(_userProvider.ContextUser.IsInRole(Roles.FileArchiveCreate) &&
                _userProvider.ContextUser.IsInRole(Roles.FileArchiveEdit)))
                query = query.Where(a => a.IsActive);

            var queryVm = query.ProjectTo<GetListFileArchiveViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.CreatedAt);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            foreach (var allFileArchiveViewModel in pagedList.Items)
            {
                allFileArchiveViewModel.FilePath = FileService.GetFileArchivePath(allFileArchiveViewModel.FilePath);
            }

            return BaseDataResponse<PagedList<GetListFileArchiveViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<GetByIdFileArchiveViewModel>> CreateAsync(CreateFileArchiveViewModel createFileArchive)
        {
            var existsFileArchive = await _unitOfWork.FileArchives.GetDbSet()
                .FirstOrDefaultAsync(p => p.Title.ToLower() == createFileArchive.Title.ToLower());

            if(existsFileArchive != null)
                return BaseDataResponse<GetByIdFileArchiveViewModel>.Fail(
                    _mapper.Map<GetByIdFileArchiveViewModel>(createFileArchive),
                    new ErrorModel(ErrorCode.NameMustBeUnique));
            
            var fileArchive = _mapper.Map<FileArchive>(createFileArchive);

            if (createFileArchive.File != null)
                fileArchive.FilePath = await _fileService.SaveFile(FileService.FileArchiveFolderPath, createFileArchive.File);

            _unitOfWork.FileArchives.Add(fileArchive);

            await _unitOfWork.CommitAsync();

            return await GetByIdAsync(fileArchive.Id);
        }

        public async Task<BaseDataResponse<GetByIdFileArchiveViewModel>> EditAsync(EditFileArchiveViewModel editFileArchiveViewModel)
        {

            var existFileArchive = await GetQuery().FirstOrDefaultAsync(p => p.Id == editFileArchiveViewModel.Id);
            if (existFileArchive == null)
                return BaseDataResponse<GetByIdFileArchiveViewModel>.NotFound(null);
           
            var checkNameUnique = await GetQuery()
                .AnyAsync(d =>
                    d.Id != editFileArchiveViewModel.Id &&
                    d.Title.ToLower() == editFileArchiveViewModel.Title.ToLower());
            if (checkNameUnique)
                return BaseDataResponse<GetByIdFileArchiveViewModel>.Fail(_mapper.Map<GetByIdFileArchiveViewModel>(editFileArchiveViewModel),
                    new ErrorModel(ErrorCode.NameMustBeUnique));
            
            var fileArchive = _mapper.Map<EditFileArchiveViewModel, FileArchive>(editFileArchiveViewModel, existFileArchive);

            if (editFileArchiveViewModel.File != null)
                fileArchive.FilePath = await _fileService.SaveFile(FileService.FileArchiveFolderPath, editFileArchiveViewModel.File);

            _unitOfWork.FileArchives.Update(fileArchive);

            await _unitOfWork.CommitAsync();

            return await GetByIdAsync(fileArchive.Id);
        }
    }
}
