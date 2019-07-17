using System;
using System.Collections.Generic;
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
            return _unitOfWork.FileArchives.GetDbSet();
        }

        private IQueryable<FileArchive> GetQueryWithUser()
        {
            return GetQuery().Include(u => u.CreatedUser);
        }

        private IQueryable<FileArchive> FilterQuery(FilterFileArchiveViewModel filterFileArchiveViewModel, IQueryable<FileArchive> query)
        {
            if (filterFileArchiveViewModel.ShortDescription != null)
            {
                query = query.Where(f => EF.Functions.Like( filterFileArchiveViewModel.ShortDescription.ToLower(), f.ShortDescription.ToLower()));
            }

            if (filterFileArchiveViewModel.IsActive)
            {
                query = query.Where(n => n.IsActive);
            }

            if (filterFileArchiveViewModel.FileCategoryId.IsEmpty())
            {
                query = query.Where(f => f.FileCategoryId == filterFileArchiveViewModel.FileCategoryId);
            }

            return query;
        }

        public async Task<BaseDataResponse<FileArchiveViewModel>> GetByIdAsync(Guid id)
        {
            var existsFileArchive = await GetQuery().FirstOrDefaultAsync(u => u.Id == id);
            if (existsFileArchive == null)
            {
                return BaseDataResponse<FileArchiveViewModel>.NotFound(null);
            }

            return BaseDataResponse<FileArchiveViewModel>.Success(_mapper.Map<FileArchiveViewModel>(existsFileArchive));
        }

        public async Task<BaseDataResponse<PagedList<FileArchiveViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterFileArchiveViewModel filterFileArchiveViewModel, SortRule sortRule)
        {
            var query = GetQueryWithUser().Include(c => c.FileCategory).AsQueryable();

            query = FilterQuery(filterFileArchiveViewModel, query);

            var queryVm = query.ProjectTo<FileArchiveViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.ShortDescription);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<FileArchiveViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllSelectListItemAsync()
        {
            var listItem = await GetQuery().Where(p => p.IsActive).ToListAsync();

            return BaseDataResponse<IEnumerable<SelectListItemViewModel>>.Success(
                _mapper.Map<IEnumerable<SelectListItemViewModel>>(listItem));
        }

        public async Task<BaseDataResponse<EditFileArchiveViewModel>> CreateAsync(EditFileArchiveViewModel editFileArchiveViewModel)
        {
            BaseDataResponse<EditFileArchiveViewModel> response;

            var existsFileArchive = await GetQuery()
                .FirstOrDefaultAsync(p => p.ShortDescription.ToLower() == editFileArchiveViewModel.ShortDescription.ToLower());
            if (existsFileArchive != null)
            {
                response = BaseDataResponse<EditFileArchiveViewModel>.Fail(editFileArchiveViewModel, new ErrorModel("Entity  was found with the same name!"));
            }
            else
            {
                var fileArchive = _mapper.Map<FileArchive>(editFileArchiveViewModel);

                if (editFileArchiveViewModel.File != null)
                {
                    fileArchive.FileName = await _fileService.SaveFile(FileService.FileArchiveFolderPath, editFileArchiveViewModel.File);
                }

                _unitOfWork.FileArchives.Add(fileArchive);

                await _unitOfWork.CommitAsync();

                response = BaseDataResponse<EditFileArchiveViewModel>.Success(_mapper.Map<EditFileArchiveViewModel>(fileArchive));
            }

            return response;
        }

        //public async Task<BaseDataResponse<VIEWMODEL>> EditAsync(VIEWMODEL editVIEWMODEL)
        //{
        //    BaseDataResponse<VIEWMODEL> response;

        //    var existENTITY = await GetQuery().FirstOrDefaultAsync(p => p.Id == editVIEWMODEL.Id);
        //    if (existENTITY == null)
        //    {
        //        response = BaseDataResponse<VIEWMODEL>.NotFound(editVIEWMODEL);
        //    }
        //    else
        //    {
        //        var checkNameUnique = await GetQuery()
        //            .AnyAsync(d => d.Id != editVIEWMODEL.Id && d.Name.ToLower() == editVIEWMODEL.Name.ToLower());
        //        if (checkNameUnique)
        //        {
        //            response = BaseDataResponse<VIEWMODEL>.Fail(editVIEWMODEL,
        //                new ErrorModel("We have already this useful link category"));
        //        }
        //        else
        //        {
        //            var VARENTITY = _mapper.Map<VIEWMODEL, ENTITY>(editVIEWMODEL, existENTITY);

        //            _unitOfWork.ENTITY.Update(VARENTITY);

        //            await _unitOfWork.CommitAsync();

        //            response = BaseDataResponse<VIEWMODEL>.Success(_mapper.Map<VIEWMODEL>(VARENTITY));
        //        }
        //    }

        //    return response;
        //}
    }
}
