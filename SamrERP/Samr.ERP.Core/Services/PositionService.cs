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
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Position;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class PositionService : IPositionService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PositionService(IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        private IQueryable<Position> GetQueryWithUser()
        {
            return _unitOfWork.Positions.GetDbSet()
                .OrderByDescending(p => p.CreatedAt)
                .Include(p => p.CreatedUser)
                .ThenInclude( p => p.Employee);
        }

        private IQueryable<Position> GetAllQuery(bool onlyActives = false)
        {
            return _unitOfWork.Positions.All()
                .Where(p => !onlyActives || p.IsActive)
                .Include(p => p.Department)
                .Include(p => p.CreatedUser)
                .ThenInclude(p => p.Employee);
        }

        private IQueryable<Position> FilterQuery(FilterPositionViewModel filterPosition, IQueryable<Position> query)
        {
            if (filterPosition.Name != null)
            {
                query = query.Where(n => EF.Functions.Like(n.Name, "%" + filterPosition.Name + "%"));
            }

            if (filterPosition.OnlyActive)
            {
                query = query.Where(n => n.IsActive);
            }

            if (filterPosition.DepartmentId != null)
            {
                query = query.Where(n => n.DepartmentId == filterPosition.DepartmentId);
            }

            return query;
        }

        public async Task<BaseDataResponse<ResponsePositionViewModel>> GetByIdAsync(Guid id)
        {
            var position =  await GetQueryWithUser()
                .Include( p => p.Department)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (position == null)
            {
                return BaseDataResponse<ResponsePositionViewModel>.NotFound(null);
            }

            var positionLog = await _unitOfWork.PositionLogs.GetDbSet()
                .Include( p => p.CreatedUser)
                .ThenInclude( p => p.Employee)
                .OrderByDescending( p => p.CreatedAt)
                .FirstOrDefaultAsync(p => p.PositionId == position.Id);

            if (positionLog != null )
            {
                position.CreatedUser = positionLog.CreatedUser;
                position.CreatedAt = positionLog.CreatedAt;
            }

            return BaseDataResponse<ResponsePositionViewModel>.Success(
                _mapper.Map<ResponsePositionViewModel>(position));
        }

        public async Task<BaseDataResponse<PagedList<ResponsePositionViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterPositionViewModel filterPosition, SortRule sortRule)
        {
            var query = GetAllQuery();

            query = FilterQuery(filterPosition, query);

            var queryVm = query.ProjectTo<ResponsePositionViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<ResponsePositionViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<IEnumerable<PositionViewModel>>> GetAllByDepartmentId(Guid id)
        {
            var positions = await GetAllQuery(true).Where(p=>p.DepartmentId == id).ToListAsync();
            var vm = _mapper.Map<IEnumerable<PositionViewModel>>(positions);

            var response = BaseDataResponse<IEnumerable<PositionViewModel>>.Success(vm);

            return response;
        }

        public async Task<BaseDataResponse<ResponsePositionViewModel>> CreateAsync(RequestPositionViewModel positionViewModel)
        {

            var positionExists =
                _unitOfWork.Positions.Any(p => p.Name.ToLower() == positionViewModel.Name.ToLower());
            if (positionExists)
            {
                    return BaseDataResponse<ResponsePositionViewModel>.Fail(
                        _mapper.Map<ResponsePositionViewModel>(positionViewModel),
                        new ErrorModel(ErrorCode.NameMustBeUnique));
            }

            var position = _mapper.Map<Position>(positionViewModel);
            _unitOfWork.Positions.Add(position);

            await _unitOfWork.CommitAsync();

            return await GetByIdAsync(position.Id);
        }

        public async Task<BaseDataResponse<ResponsePositionViewModel>> EditAsync(RequestPositionViewModel positionViewModel)
        {
            var positionExists = await _unitOfWork.Positions.GetDbSet().FirstOrDefaultAsync( p => p.Id == positionViewModel.Id);
            if (positionExists == null)
            {
                return BaseDataResponse<ResponsePositionViewModel>.NotFound(
                    _mapper.Map<ResponsePositionViewModel>(positionViewModel));
            }

            var checkNameUnique = await _unitOfWork.Positions.GetDbSet()
                .AnyAsync(p => p.Id != positionViewModel.Id 
                               && p.Name.ToLower() == positionViewModel.Name.ToLower());
            if (checkNameUnique)
            {
                return BaseDataResponse<ResponsePositionViewModel>.Fail(
                    _mapper.Map<ResponsePositionViewModel>(positionViewModel),
                    new ErrorModel(ErrorCode.NameMustBeUnique));
            }

            var positionLog = _mapper.Map<PositionLog>(positionExists);
            _unitOfWork.PositionLogs.Add(positionLog);

            var position = _mapper.Map(positionViewModel, positionExists);

            _unitOfWork.Positions.Update(position);

            await _unitOfWork.CommitAsync();

            return await GetByIdAsync(position.Id);
        }

        public async Task<BaseDataResponse<PagedList<PositionLogViewModel>>> GetAllLogAsync(Guid id, PagingOptions pagingOptions, SortRule sortRule)
        {
            var query = _unitOfWork.PositionLogs.GetDbSet().Include(p => p.CreatedUser).Where(d => d.PositionId == id).OrderByDescending(p => p.CreatedAt);

            var queryVm = query.ProjectTo<PositionLogViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<PositionLogViewModel>>.Success(pagedList);
        }
    }
}
