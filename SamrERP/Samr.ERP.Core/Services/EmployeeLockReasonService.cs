using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class EmployeeLockReasonService : IEmployeeLockReasonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeLockReasonService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<EditEmployeeLockReasonViewModel>> GetByIdAsync(Guid id)
        {
            var employeeLockReason = await _unitOfWork.EmployeeLockReasons.GetDbSet()
                .Include(p => p.CreatedUser)
                .FirstOrDefaultAsync(p => p.Id == id);

            BaseResponse<EditEmployeeLockReasonViewModel> response;

            if (employeeLockReason == null)
            {
                response = BaseResponse<EditEmployeeLockReasonViewModel>.NotFound( null);
            }
            else
            {
                response = BaseResponse<EditEmployeeLockReasonViewModel>.Success(_mapper.Map<EditEmployeeLockReasonViewModel>(employeeLockReason));
            }

            return response;
        }

        public async Task<BaseResponse<IEnumerable<EmployeeLockReasonViewModel>>> GetAll()
        {
            var employeeLockReason = await _unitOfWork.EmployeeLockReasons.GetDbSet().Include(p => p.CreatedUser).ToListAsync();
            var vm = _mapper.Map<IEnumerable<EmployeeLockReasonViewModel>>(employeeLockReason);

            var response = BaseResponse<IEnumerable<EmployeeLockReasonViewModel>>.Success(vm);

            return response;
        }

        public async Task<BaseResponse<EditEmployeeLockReasonViewModel>> CreateAsync(EditEmployeeLockReasonViewModel employeeLockReasonViewModel)
        {
            BaseResponse<EditEmployeeLockReasonViewModel> response;

            var employeeLockReason =
                _unitOfWork.Departments.Any(p => p.Name.ToLower() == employeeLockReasonViewModel.Name.ToLower());
            if (employeeLockReason)
            {
                response = BaseResponse<EditEmployeeLockReasonViewModel>.Fail(employeeLockReasonViewModel, new ErrorModel("Already this model in database."));
            }
            else
            {
                _unitOfWork.EmployeeLockReasons.Add(_mapper.Map<EmployeeLockReason>(employeeLockReasonViewModel));
                response = BaseResponse<EditEmployeeLockReasonViewModel>.Success(employeeLockReasonViewModel);
                await _unitOfWork.CommitAsync();
            }

            return response;
        }

        public async Task<BaseResponse<EditEmployeeLockReasonViewModel>> UpdateAsync(EditEmployeeLockReasonViewModel employeeLockReasonViewModel)
        {
            BaseResponse<EditEmployeeLockReasonViewModel> response;

            var employeeLockReasonExists = await _unitOfWork.Departments.ExistsAsync(employeeLockReasonViewModel.Id);
            if (employeeLockReasonExists)
            {
                var employeeLockReason = _mapper.Map<EmployeeLockReason>(employeeLockReasonViewModel);

                _unitOfWork.EmployeeLockReasons.Update(employeeLockReason);

                await _unitOfWork.CommitAsync();

                response = BaseResponse<EditEmployeeLockReasonViewModel>.Success(_mapper.Map<EditEmployeeLockReasonViewModel>(employeeLockReason));
            }
            else
            {
                response = BaseResponse<EditEmployeeLockReasonViewModel>.NotFound(employeeLockReasonViewModel);
            }

            return response;
        }
    }
}
