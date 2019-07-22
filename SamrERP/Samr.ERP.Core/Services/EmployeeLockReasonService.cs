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
using Samr.ERP.Core.ViewModels.Handbook.EmployeeLockReason;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class EmployeeLockReasonService : IEmployeeLockReasonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHandbookService _handbookService;

        public EmployeeLockReasonService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHandbookService handbookService
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _handbookService = handbookService;
        }

        private IQueryable<EmployeeLockReason> GetQuery()
        {
            return _unitOfWork.EmployeeLockReasons.GetDbSet();
        }

        private IQueryable<EmployeeLockReason> GetQueryWithUser()
        {
            return GetQuery().Include(p => p.CreatedUser);
        }

        private bool EmployeeLockReasonExists(EditEmployeeLockReasonViewModel employeeLockReasonViewModel)
        {
            return _unitOfWork.EmployeeLockReasons.Any(p => p.Name.ToLower() == employeeLockReasonViewModel.Name.ToLower());
        }

        private IQueryable<EmployeeLockReason> FilterQuery(FilterHandbookViewModel filterHandbook, IQueryable<EmployeeLockReason> query)
        {
            if (filterHandbook.Name != null)
            {
                query = query.Where(n => n.Name == filterHandbook.Name);
            }

            if (filterHandbook.OnlyActive)
            {
                query = query.Where(n => n.IsActive);
            }

            return query;
        }

        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllListItemAsync()
        {
            var listItem = await GetQuery().Where(e => e.IsActive).ToListAsync();

            var vm = _mapper.Map<IEnumerable<SelectListItemViewModel>>(listItem);

            return BaseDataResponse<IEnumerable<SelectListItemViewModel>>.Success(vm);
        }
        public async Task<BaseDataResponse<EditEmployeeLockReasonViewModel>> GetByIdAsync(Guid id)
        {
            var employeeLockReason = await GetQuery().FirstOrDefaultAsync(p => p.Id == id);

            BaseDataResponse<EditEmployeeLockReasonViewModel> dataResponse;

            if (employeeLockReason == null)
            {
                dataResponse = BaseDataResponse<EditEmployeeLockReasonViewModel>.NotFound( null);
            }
            else
            {
                dataResponse = BaseDataResponse<EditEmployeeLockReasonViewModel>.Success(_mapper.Map<EditEmployeeLockReasonViewModel>(employeeLockReason));
            }

            return dataResponse;
        }

        public async Task<BaseDataResponse<PagedList<EmployeeLockReasonViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule)
        {
            var query = GetQueryWithUser();

            query = FilterQuery(filterHandbook, query);

            var queryVm = query.ProjectTo<EmployeeLockReasonViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<EmployeeLockReasonViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<EditEmployeeLockReasonViewModel>> CreateAsync(EditEmployeeLockReasonViewModel employeeLockReasonViewModel)
        {
            BaseDataResponse<EditEmployeeLockReasonViewModel> dataResponse;

            var employeeLockReasonExists = EmployeeLockReasonExists(employeeLockReasonViewModel);
            if (employeeLockReasonExists)
            {
                dataResponse = BaseDataResponse<EditEmployeeLockReasonViewModel>.Fail(employeeLockReasonViewModel, new ErrorModel("Already this model in database."));
            }
            else
            {
                var employeeLockReason = _mapper.Map<EmployeeLockReason>(employeeLockReasonViewModel);
                _unitOfWork.EmployeeLockReasons.Add(employeeLockReason);

                var handbookExists = await _handbookService.ChangeStatus("Nationality", employeeLockReason.CreatedUserId);
                if (handbookExists)
                {
                    await _unitOfWork.CommitAsync();

                    dataResponse = BaseDataResponse<EditEmployeeLockReasonViewModel>.Success(_mapper.Map<EditEmployeeLockReasonViewModel>(employeeLockReason));
                }
                else
                {
                    dataResponse = BaseDataResponse<EditEmployeeLockReasonViewModel>.Fail(employeeLockReasonViewModel, new ErrorModel("Not found handbook"));
                }

                dataResponse = BaseDataResponse<EditEmployeeLockReasonViewModel>.Success(_mapper.Map<EditEmployeeLockReasonViewModel>(employeeLockReason));
            }

            return dataResponse;
        }

        public async Task<BaseDataResponse<EditEmployeeLockReasonViewModel>> EditAsync(EditEmployeeLockReasonViewModel employeeLockReasonViewModel)
        {
            BaseDataResponse<EditEmployeeLockReasonViewModel> dataResponse;

            var employeeLockReasonExists = await GetQuery()
                .FirstOrDefaultAsync(e => e.Id == employeeLockReasonViewModel.Id);
            if (employeeLockReasonExists != null)
            {
                var checkNameUnique = await _unitOfWork.EmployeeLockReasons
                    .GetDbSet()
                    .AnyAsync(e => e.Id != employeeLockReasonViewModel.Id
                                   && e.Name == employeeLockReasonViewModel.Name);
                if (checkNameUnique)
                {
                    dataResponse = BaseDataResponse<EditEmployeeLockReasonViewModel>.NotFound(employeeLockReasonViewModel, new ErrorModel("Duplicate name of employee lock reason"));
                }
                else
                {
                    var employeeLockReason = _mapper.Map<EditEmployeeLockReasonViewModel, EmployeeLockReason>(employeeLockReasonViewModel, employeeLockReasonExists);

                    _unitOfWork.EmployeeLockReasons.Update(employeeLockReason);

                    var handbookExists = await _handbookService.ChangeStatus("EmployeeLockReason", employeeLockReason.CreatedUserId);
                    if (handbookExists)
                    {
                        await _unitOfWork.CommitAsync();

                        dataResponse = BaseDataResponse<EditEmployeeLockReasonViewModel>.Success(_mapper.Map<EditEmployeeLockReasonViewModel>(employeeLockReason));
                    }
                    else
                    {
                        dataResponse = BaseDataResponse<EditEmployeeLockReasonViewModel>.Fail(employeeLockReasonViewModel, new ErrorModel("Not found handbook"));
                    }
                }
            }
            else
            {
                dataResponse = BaseDataResponse<EditEmployeeLockReasonViewModel>.NotFound(employeeLockReasonViewModel);
            }

            return dataResponse;
        }
    }
}
