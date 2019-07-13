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
using Samr.ERP.Core.ViewModels.UsefulLink;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class UsefulLinkService : IUsefulLinkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsefulLinkService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private DbSet<UsefulLink> GetQuery()
        {
            return _unitOfWork.UsefulLinks.GetDbSet();
        }

        public async Task<BaseDataResponse<UsefulLinkViewModel>> GetByIdAsync(Guid id)
        {
            var existsUsefulLink = await GetQuery().FirstOrDefaultAsync(u => u.Id == id);
            if (existsUsefulLink == null)
            {
                return BaseDataResponse<UsefulLinkViewModel>.NotFound(null);
            }

            return BaseDataResponse<UsefulLinkViewModel>.Success(_mapper.Map<UsefulLinkViewModel>(existsUsefulLink));
        }

        public async Task<BaseDataResponse<PagedList<UsefulLinkViewModel>>> GetAllAsync(PagingOptions pagingOptions)
        {
            var pagedList = await GetQuery().ToMappedPagedListAsync<UsefulLink, UsefulLinkViewModel>(pagingOptions);

            return BaseDataResponse<PagedList<UsefulLinkViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllSelectListItemAsync()
        {
            var listItem = await GetQuery().Where(p => p.IsActive).ToListAsync();

            return BaseDataResponse<IEnumerable<SelectListItemViewModel>>.Success(
                _mapper.Map<IEnumerable<SelectListItemViewModel>>(listItem));
        }

        public async Task<BaseDataResponse<UsefulLinkViewModel>> CreateAsync(UsefulLinkViewModel editUsefulLinkViewModel)
        {
            BaseDataResponse<UsefulLinkViewModel> response;

            var existsUsefulLink = await GetQuery()
                .FirstOrDefaultAsync(p => p.ShortDescription.ToLower() == editUsefulLinkViewModel.ShortDescription.ToLower());
            if (existsUsefulLink != null)
            {
                response = BaseDataResponse<UsefulLinkViewModel>.Fail(editUsefulLinkViewModel, new ErrorModel("Entity  was found with the same name!"));
            }
            else
            {
                var usefulLink = _mapper.Map<UsefulLink>(editUsefulLinkViewModel);

                _unitOfWork.UsefulLinks.Add(usefulLink);

                await _unitOfWork.CommitAsync();

                response = BaseDataResponse<UsefulLinkViewModel>.Success(_mapper.Map<UsefulLinkViewModel>(usefulLink));
            }

            return response;
        }

        public async Task<BaseDataResponse<UsefulLinkViewModel>> EditAsync(UsefulLinkViewModel editUsefulLinkViewModel)
        {
            BaseDataResponse<UsefulLinkViewModel> response;

            var existUsefulLink = await GetQuery().FirstOrDefaultAsync(p => p.Id == editUsefulLinkViewModel.Id);
            if (existUsefulLink == null)
            {
                response = BaseDataResponse<UsefulLinkViewModel>.NotFound(editUsefulLinkViewModel);
            }
            else
            {
                var checkNameUnique = await GetQuery()
                    .AnyAsync(d => d.Id != editUsefulLinkViewModel.Id && d.ShortDescription.ToLower() == editUsefulLinkViewModel.ShortDescription.ToLower());
                if (checkNameUnique)
                {
                    response = BaseDataResponse<UsefulLinkViewModel>.Fail(editUsefulLinkViewModel,
                        new ErrorModel("We have already this useful link category"));
                }
                else
                {
                    var usefulLink = _mapper.Map<UsefulLinkViewModel, UsefulLink>(editUsefulLinkViewModel, existUsefulLink);

                    _unitOfWork.UsefulLinks.Update(usefulLink);

                    await _unitOfWork.CommitAsync();

                    response = BaseDataResponse<UsefulLinkViewModel>.Success(_mapper.Map<UsefulLinkViewModel>(usefulLink));
                }
            }

            return response;
        }
    }
}