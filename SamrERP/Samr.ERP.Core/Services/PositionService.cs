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
            return _unitOfWork.Positions.GetDbSet().Include(p => p.CreatedUser);
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

        public async Task<BaseDataResponse<EditPositionViewModel>> GetByIdAsync(Guid id)
        {
            var position = await GetQueryWithUser().FirstOrDefaultAsync(p => p.Id == id);

            BaseDataResponse<EditPositionViewModel> dataResponse;

            if (position == null)
            {
                dataResponse = BaseDataResponse<EditPositionViewModel>.NotFound(null);
            }
            else
            {
                dataResponse = BaseDataResponse<EditPositionViewModel>.Success(_mapper.Map<EditPositionViewModel>(position));
            }

            return dataResponse;
        }

        private IQueryable<Position> GetAllQuery(bool onlyActives = false)
        {
            return _unitOfWork.Positions.All()
                .Where(p=> !onlyActives || p.IsActive )
                .Include(p => p.CreatedUser)
                .Include(p => p.Department);
        }

        public async Task<BaseDataResponse<PagedList<EditPositionViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterPositionViewModel filterPosition, SortRule sortRule)
        {
            var query = GetAllQuery();

            query = FilterQuery(filterPosition, query);

            var queryVm = query.ProjectTo<EditPositionViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.Name);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<EditPositionViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<IEnumerable<PositionViewModel>>> GetAllByDepartmentId(Guid id)
        {
            var positions = await GetAllQuery(true).Where(p=>p.DepartmentId == id).ToListAsync();
            var vm = _mapper.Map<IEnumerable<PositionViewModel>>(positions);

            var response = BaseDataResponse<IEnumerable<PositionViewModel>>.Success(vm);

            return response;
        }

        public async Task<BaseDataResponse<EditPositionViewModel>> CreateAsync(EditPositionViewModel positionViewModel)
        {
            BaseDataResponse<EditPositionViewModel> dataResponse;

            var positionExists =
                _unitOfWork.Positions.Any(p => p.Name.ToLower() == positionViewModel.Name.ToLower());
            if (positionExists)
            {
                dataResponse = BaseDataResponse<EditPositionViewModel>.Fail(positionViewModel, new ErrorModel("Already this model in database."));
            }
            else
            {
                var position = _mapper.Map<Position>(positionViewModel);
                _unitOfWork.Positions.Add(position);

                await _unitOfWork.CommitAsync();

                dataResponse = BaseDataResponse<EditPositionViewModel>.Success(_mapper.Map<EditPositionViewModel>(position));
            }

            return dataResponse;
        }

        public async Task<BaseDataResponse<EditPositionViewModel>> EditAsync(EditPositionViewModel positionViewModel)
        {
            BaseDataResponse<EditPositionViewModel> dataResponse;

            var positionExists = await _unitOfWork.Positions.GetDbSet().FirstOrDefaultAsync( p => p.Id == positionViewModel.Id);
            if (positionExists != null)
            {
                var checkNameUnique = await _unitOfWork.Positions.GetDbSet()
                    .AnyAsync(p => p.Id != positionViewModel.Id 
                                   && p.Name.ToLower() == positionViewModel.Name.ToLower());
                if (checkNameUnique)
                {
                    dataResponse = BaseDataResponse<EditPositionViewModel>.Fail(positionViewModel, new ErrorModel("Duplicate position name"));
                }
                else
                {
                    var positionLog = _mapper.Map<PositionLog>(positionExists);
                    _unitOfWork.PositionLogs.Add(positionLog);

                    var position = _mapper.Map<EditPositionViewModel, Position>(positionViewModel, positionExists);

                    _unitOfWork.Positions.Update(position);

                    await _unitOfWork.CommitAsync();

                    dataResponse = BaseDataResponse<EditPositionViewModel>.Success(_mapper.Map<EditPositionViewModel>(position));
                }
            }
            else
            {
                dataResponse = BaseDataResponse<EditPositionViewModel>.NotFound(positionViewModel);
            }

            return dataResponse;
        }
    }
}
