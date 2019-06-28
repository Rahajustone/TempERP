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
        public async Task<BaseResponse<EditPositionViewModel>> GetByIdAsync(Guid id)
        {
            var position = await _unitOfWork.Positions.GetDbSet()
                .Include(p => p.CreatedUser)
                .FirstOrDefaultAsync(p => p.Id == id);

            BaseResponse<EditPositionViewModel> response;

            if (position == null)
            {
                response = BaseResponse<EditPositionViewModel>.NotFound(null);
            }
            else
            {
                response = BaseResponse<EditPositionViewModel>.Success(_mapper.Map<EditPositionViewModel>(position));
            }

            return response;
        }

        public async Task<BaseResponse<IEnumerable<PositionViewModel>>> GetAllAsync()
        {
            var positions = await _unitOfWork.Positions.GetDbSet().Include(p => p.CreatedUser).ToListAsync();
            var vm = _mapper.Map<IEnumerable<PositionViewModel>>(positions);

            var response = BaseResponse<IEnumerable<PositionViewModel>>.Success(vm);

            return response;
        }

        public async Task<BaseResponse<EditPositionViewModel>> CreateAsync(EditPositionViewModel positionViewModel)
        {
            BaseResponse<EditPositionViewModel> response;

            var positionExists =
                _unitOfWork.Positions.Any(p => p.Name.ToLower() == positionViewModel.Name.ToLower());
            if (positionExists)
            {
                response = BaseResponse<EditPositionViewModel>.Fail(positionViewModel, new ErrorModel("Already this model in database."));
            }
            else
            {
                _unitOfWork.Positions.Add(_mapper.Map<Position>(positionViewModel));
                response = BaseResponse<EditPositionViewModel>.Success(positionViewModel);
                await _unitOfWork.CommitAsync();
            }

            return response;
        }

        public async Task<BaseResponse<EditPositionViewModel>> UpdateAsync(EditPositionViewModel positionViewModel)
        {
            BaseResponse<EditPositionViewModel> response;

            var positionExists = await _unitOfWork.Positions.ExistsAsync(positionViewModel.Id);
            if (positionExists)
            {
                var position = _mapper.Map<Position>(positionViewModel);

                _unitOfWork.Positions.Update(position);

                await _unitOfWork.CommitAsync();

                response = BaseResponse<EditPositionViewModel>.Success(_mapper.Map<EditPositionViewModel>(position));
            }
            else
            {
                response = BaseResponse<EditPositionViewModel>.NotFound(positionViewModel);
            }

            return response;
        }
    }
}
