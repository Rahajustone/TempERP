using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.Position;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class PositionService : IPositionService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PositionService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseDataResponse<EditPositionViewModel>> GetByIdAsync(Guid id)
        {
            var position = await _unitOfWork.Positions.GetDbSet()
                .Include(p => p.CreatedUser)
                .FirstOrDefaultAsync(p => p.Id == id);

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

        public async Task<BaseDataResponse<IEnumerable<PositionViewModel>>> GetAllAsync()
        {
            var positions = await _unitOfWork.Positions.GetDbSet().Include(p => p.CreatedUser).ToListAsync();
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

                dataResponse = BaseDataResponse<EditPositionViewModel>.Success(_mapper.Map<EditPositionViewModel>(position));

                await _unitOfWork.CommitAsync();
            }

            return dataResponse;
        }

        public async Task<BaseDataResponse<EditPositionViewModel>> UpdateAsync(EditPositionViewModel positionViewModel)
        {
            BaseDataResponse<EditPositionViewModel> dataResponse;

            var positionExists = await _unitOfWork.Positions.ExistsAsync(positionViewModel.Id);
            if (positionExists)
            {
                var position = _mapper.Map<Position>(positionViewModel);

                _unitOfWork.Positions.Update(position);

                await _unitOfWork.CommitAsync();

                dataResponse = BaseDataResponse<EditPositionViewModel>.Success(_mapper.Map<EditPositionViewModel>(position));
            }
            else
            {
                dataResponse = BaseDataResponse<EditPositionViewModel>.NotFound(positionViewModel);
            }

            return dataResponse;
        }
    }
}
