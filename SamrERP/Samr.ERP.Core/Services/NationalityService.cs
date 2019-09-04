using System;
using System.Collections.Generic;
using System.Linq;
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
using Samr.ERP.Core.ViewModels.Handbook.Nationality;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class NationalityService : INationalityService 
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NationalityService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private IQueryable<Nationality> GetQuery()
        {
            return _unitOfWork.Nationalities.GetDbSet().OrderByDescending( p => p.CreatedAt);
        }

        private IQueryable<Nationality> GetQueryWithUser()
        {
            return GetQuery().Include(u => u.CreatedUser).ThenInclude( p => p.Employee );
        }

        private IQueryable<Nationality> FilterQuery(FilterHandbookViewModel filterHandbook, IQueryable<Nationality> query)
        {
            if (filterHandbook.Name != null)
            {
                query = query.Where(n => EF.Functions.Like(n.Name, "%" + filterHandbook.Name + "%"));
            }

            if (filterHandbook.OnlyActive)
            {
                query = query.Where(n => n.IsActive);
            }

            return query;
        }

        public async Task<BaseDataResponse<PagedList<ResponseNationalityViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule)
        {
            var query = GetQueryWithUser();

            query = FilterQuery(filterHandbook, query);

            var queryVm = query.ProjectTo<ResponseNationalityViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<ResponseNationalityViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllListItemAsync()
        {
            var listItems = await GetQuery().Where(n => n.IsActive).ToListAsync();

            return BaseDataResponse<IEnumerable<SelectListItemViewModel>>.Success(_mapper.Map<IEnumerable<SelectListItemViewModel>>(listItems));
        }

        public async Task<BaseDataResponse<ResponseNationalityViewModel>> GetByIdAsync(Guid id)
        {

            var nationality = await GetQueryWithUser().FirstOrDefaultAsync(p => p.Id == id);
            if (nationality == null)
            {
                return  BaseDataResponse<ResponseNationalityViewModel>.NotFound(null, new ErrorModel("Not found Entity"));
            }

            var nationalityLog = await _unitOfWork.NationalityLogs.GetDbSet()
                .Include(p => p.CreatedUser)
                .ThenInclude(p => p.Employee)
                .OrderByDescending( p => p.CreatedAt)
                .FirstOrDefaultAsync(n => n.NationalityId == nationality.Id);

            if (nationalityLog != null)
            {
                nationality.CreatedAt = nationalityLog.CreatedAt;
                nationality.CreatedUser = nationalityLog.CreatedUser;
            }

            return BaseDataResponse<ResponseNationalityViewModel>.Success(_mapper.Map<ResponseNationalityViewModel>(nationality));
        }

        public async Task<BaseDataResponse<ResponseNationalityViewModel>> CreateAsync(RequestNationalityViewModel nationalityViewModel)
        {
            var nationalityExists =
                _unitOfWork.Nationalities
                    .Any(p => p.Name.ToLower() == nationalityViewModel.Name.ToLower());

            if (nationalityExists)
            {
                return BaseDataResponse<ResponseNationalityViewModel>.Fail(
                    _mapper.Map<ResponseNationalityViewModel>(nationalityViewModel),
                    new ErrorModel(ErrorCode.NameMustBeUnique));
            }

            var nationality = _mapper.Map<Nationality>(nationalityViewModel);
            _unitOfWork.Nationalities.Add(nationality);

            await _unitOfWork.CommitAsync();

            return await GetByIdAsync(nationality.Id);
        }

        public async Task<BaseDataResponse<ResponseNationalityViewModel>> EditAsync(RequestNationalityViewModel nationalityViewModel)
        {
            var nationalityExists = await GetQuery().FirstOrDefaultAsync(n => n.Id == nationalityViewModel.Id);

            if (nationalityExists == null)
            {
                return BaseDataResponse<ResponseNationalityViewModel>.NotFound(
                    _mapper.Map<ResponseNationalityViewModel>(nationalityViewModel));
            }

            var checkNameUnique = await _unitOfWork.Nationalities.GetDbSet()
                .AnyAsync(n =>
                    n.Id != nationalityViewModel.Id && n.Name.ToLower() == nationalityViewModel.Name.ToLower());
            if (checkNameUnique)
            {
                return  BaseDataResponse<ResponseNationalityViewModel>.Fail(_mapper.Map<ResponseNationalityViewModel>(nationalityViewModel), 
                    new ErrorModel(ErrorCode.NameMustBeUnique));
            }

            var nationalityLog = _mapper.Map<NationalityLog>(nationalityExists);
            _unitOfWork.NationalityLogs.Add(nationalityLog);

            var nationality = _mapper.Map<RequestNationalityViewModel, Nationality>(nationalityViewModel, nationalityExists);
            _unitOfWork.Nationalities.Update(nationality);
            
            await _unitOfWork.CommitAsync();

            return await GetByIdAsync(nationalityExists.Id);
        }

        public async Task<BaseDataResponse<PagedList<NationalityLogViewModel>>> GetAllLogAsync(Guid id, PagingOptions pagingOptions, SortRule sortRule)
        {
            var query = _unitOfWork.NationalityLogs.GetDbSet().OrderByDescending( p => p.CreatedAt).Where(p => p.NationalityId == id);

            var queryVm = query.ProjectTo<NationalityLogViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<NationalityLogViewModel>>.Success(pagedList);
        }
    }
}
