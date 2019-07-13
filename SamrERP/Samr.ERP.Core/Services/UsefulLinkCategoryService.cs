﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Department;
using Samr.ERP.Core.ViewModels.UsefulLink.UsefulLinkCategory;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class UsefulLinkCategoryService : IUsefulLinkCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsefulLinkCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        private DbSet<UsefulLinkCategory> GetQuery()
        {
            return _unitOfWork.UsefulLinkCategories.GetDbSet();
        }

        public async Task<BaseDataResponse<EditUsefulLinkCategoryViewModel>> GetByIdAsync(Guid id)
        {
            var existUsefulLinkCategory = await GetQuery().FirstOrDefaultAsync(u => u.Id == id);
            if (existUsefulLinkCategory == null )
            {
                return BaseDataResponse<EditUsefulLinkCategoryViewModel>.NotFound(null);
            }

            return BaseDataResponse<EditUsefulLinkCategoryViewModel>.Success(_mapper.Map<EditUsefulLinkCategoryViewModel>(existUsefulLinkCategory));
        }

        public async Task<BaseDataResponse<PagedList<EditUsefulLinkCategoryViewModel>>> GetAllAsync(PagingOptions pagingOptions)
        {
            var pagedList = await GetQuery().ToMappedPagedListAsync<UsefulLinkCategory, EditUsefulLinkCategoryViewModel>(pagingOptions);
            
            return BaseDataResponse<PagedList<EditUsefulLinkCategoryViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllSelectListItemAsync()
        {
             var listItem = await GetQuery().Where(p => p.IsActive).ToListAsync();

             return BaseDataResponse<IEnumerable<SelectListItemViewModel>>.Success(_mapper.Map<IEnumerable<SelectListItemViewModel>>(listItem));
        }

        public async Task<BaseDataResponse<EditUsefulLinkCategoryViewModel>> CreateAsync(EditUsefulLinkCategoryViewModel editUsefulLinkCategoryViewModel)
        {
            BaseDataResponse<EditUsefulLinkCategoryViewModel> response;

            var existsUsefulLinkCategories = await GetQuery().FirstOrDefaultAsync(p => p.Name.ToLower() == editUsefulLinkCategoryViewModel.Name.ToLower());
            if (existsUsefulLinkCategories != null)
            {
                response = BaseDataResponse<EditUsefulLinkCategoryViewModel>.Fail(editUsefulLinkCategoryViewModel, new ErrorModel("Entity  was found with the same name!"));
            }
            else
            {
                var usefulLinkCategory = _mapper.Map<UsefulLinkCategory>(editUsefulLinkCategoryViewModel);

                _unitOfWork.UsefulLinkCategories.Add(usefulLinkCategory);

                await _unitOfWork.CommitAsync();

                response = BaseDataResponse<EditUsefulLinkCategoryViewModel>.Success( _mapper.Map<EditUsefulLinkCategoryViewModel>(usefulLinkCategory));
            }

            return response;
        }

        public async Task<BaseDataResponse<EditUsefulLinkCategoryViewModel>> EditAsync(EditUsefulLinkCategoryViewModel editUsefulLinkCategoryViewModel)
        {
            BaseDataResponse<EditUsefulLinkCategoryViewModel> response;

            var existUsefulLinkCategory = await GetQuery().FirstOrDefaultAsync(p => p.Id == editUsefulLinkCategoryViewModel.Id);
            if (existUsefulLinkCategory == null)
            {
                 response = BaseDataResponse<EditUsefulLinkCategoryViewModel>.NotFound(editUsefulLinkCategoryViewModel);
            }
            else
            {
                var checkNameUnique = await GetQuery()
                    .AnyAsync(d => d.Id != editUsefulLinkCategoryViewModel.Id && d.Name.ToLower() == editUsefulLinkCategoryViewModel.Name.ToLower());
                if (checkNameUnique)
                {
                    response = BaseDataResponse<EditUsefulLinkCategoryViewModel>.Fail(editUsefulLinkCategoryViewModel, new ErrorModel("We have already this useful link category"));
                }
                else
                {
                    var usefulLinkCategory = _mapper.Map<EditUsefulLinkCategoryViewModel, UsefulLinkCategory>(editUsefulLinkCategoryViewModel, existUsefulLinkCategory);

                    _unitOfWork.UsefulLinkCategories.Update(usefulLinkCategory);

                    await _unitOfWork.CommitAsync();

                    response = BaseDataResponse<EditUsefulLinkCategoryViewModel>.Success(_mapper.Map<EditUsefulLinkCategoryViewModel>(usefulLinkCategory));
                }
            }

            return response;
        }
    }
}