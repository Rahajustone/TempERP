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
using Samr.ERP.Core.ViewModels.Handbook.FileCategory;
using Samr.ERP.Core.ViewModels.Position;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class FileCategoryService : IFileCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHandbookService _handbookService;

        public FileCategoryService(IUnitOfWork unitOfWork, IMapper mapper, IHandbookService handbookService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _handbookService = handbookService;
        }

        private IQueryable<FileCategory> GetQuery()
        {
            return _unitOfWork.FileCategories.GetDbSet().Include(p => p.CreatedUser);
        }

        //private IQueryable<FileCategory> GetQuery()
        //{
        //    return GetQuery().Include(p => p.CreatedUser);
        //}

        private IQueryable<FileCategory> FilterQuery(FilterHandbookViewModel filterHandbook, IQueryable<FileCategory> query)
        {
            if (filterHandbook.Name != null)
            {
                var filter = filterHandbook.Name.ToLower();
                query = query.Where(n => EF.Functions.Like(n.Name.ToLower(), "%" + filter +"%"));
            }

            if (filterHandbook.IsActive)
            {
                query = query.Where(n => n.IsActive);
            }

            return query;
        }

        public async Task<BaseDataResponse<EditFileCategoryViewModel>> GetByIdAsync(Guid id)
        {
            var existsFileCategory = await GetQuery().FirstOrDefaultAsync(u => u.Id == id);
            if (existsFileCategory == null)
            {
                return BaseDataResponse<EditFileCategoryViewModel>.NotFound(null);
            }

            return BaseDataResponse<EditFileCategoryViewModel>.Success(_mapper.Map<EditFileCategoryViewModel>(existsFileCategory));
        }

        public async Task<BaseDataResponse<PagedList<FileCategoryViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule)
        {
            var query = GetQuery();

            query = FilterQuery(filterHandbook, query);

            var queryVm = query.ProjectTo<FileCategoryViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<FileCategoryViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllSelectListItemAsync()
        {
            var listItem = await GetQuery().Where(p => p.IsActive).ToListAsync();

            return BaseDataResponse<IEnumerable<SelectListItemViewModel>>.Success(
                _mapper.Map<IEnumerable<SelectListItemViewModel>>(listItem));
        }

        public async Task<BaseDataResponse<EditFileCategoryViewModel>> CreateAsync(EditFileCategoryViewModel fileCategoryViewModel)
        {
            BaseDataResponse<EditFileCategoryViewModel> dataResponse;

            var fileCategoryExists = _unitOfWork.FileCategories.Any(p => p.Name.ToLower() == fileCategoryViewModel.Name.ToLower());
            if (fileCategoryExists)
            {
                dataResponse = BaseDataResponse<EditFileCategoryViewModel>.Fail(fileCategoryViewModel, new ErrorModel("Already this model in database."));
            }
            else
            {
                var fileCategory = _mapper.Map<FileCategory>(fileCategoryViewModel);
                _unitOfWork.FileCategories.Add(fileCategory);

                var handbookExists = await _handbookService.ChangeStatus("FileCategory", fileCategory.CreatedUserId);
                if (handbookExists)
                {
                    await _unitOfWork.CommitAsync();

                    dataResponse = BaseDataResponse<EditFileCategoryViewModel>.Success(_mapper.Map<EditFileCategoryViewModel>(fileCategory));
                }
                else
                {
                    dataResponse = BaseDataResponse<EditFileCategoryViewModel>.Fail(fileCategoryViewModel, new ErrorModel("Not found handbook."));
                }
            }

            return dataResponse;
        }

        public async Task<BaseDataResponse<EditFileCategoryViewModel>> EditAsync(EditFileCategoryViewModel editFileCategoryViewModel)
        {
            BaseDataResponse<EditFileCategoryViewModel> dataResponse;

            var existsFileCategory = await GetQuery().FirstOrDefaultAsync(u => u.Id == editFileCategoryViewModel.Id);
            if (existsFileCategory != null)
            {
                var checkNameUnique = await GetQuery().AnyAsync(u => u.Id != editFileCategoryViewModel.Id
                                                                     && u.Name.ToLower() == editFileCategoryViewModel.Name.ToLower());
                if (checkNameUnique)
                {
                    dataResponse = BaseDataResponse<EditFileCategoryViewModel>.NotFound(editFileCategoryViewModel, new ErrorModel("Duplicate name of file category!"));
                }
                else
                {
                    var fileCategory = _mapper.Map<EditFileCategoryViewModel, FileCategory>(editFileCategoryViewModel, existsFileCategory);

                    _unitOfWork.FileCategories.Update(fileCategory);

                    var handbookExists = await _handbookService.ChangeStatus("FileCategory", fileCategory.CreatedUserId);
                    if (handbookExists)
                    {
                        await _unitOfWork.CommitAsync();

                        dataResponse = BaseDataResponse<EditFileCategoryViewModel>.Success(_mapper.Map<EditFileCategoryViewModel>(fileCategory));
                    }
                    else
                    {
                        dataResponse = BaseDataResponse<EditFileCategoryViewModel>.Fail(editFileCategoryViewModel, new ErrorModel("Not found handbook."));
                    }
                }
            }
            else
            {
                dataResponse = BaseDataResponse<EditFileCategoryViewModel>.NotFound(editFileCategoryViewModel);
            }

            return dataResponse;
        }
    }
}
