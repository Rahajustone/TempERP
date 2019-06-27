using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Interfaces;
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

        public async Task<BaseResponse<EmployeeLockReasonViewModel>> GetByIdAsync(Guid id)
        {
            var employeeLockReason =_unitOfWork.EmployeeLockReasons.GetDbSet().Include(p => p.CreatedUser).FirstOrDefault(p => p.Id == id);

            var vm = _mapper.Map<EmployeeLockReasonViewModel>(employeeLockReason);

            var response = new BaseResponse<EmployeeLockReasonViewModel>(vm, true);

            return response;
        }

        public async Task<BaseResponse<IEnumerable<EmployeeLockReasonViewModel>>> GetAll()
        {
            var employeeLockReason = _unitOfWork.EmployeeLockReasons.GetDbSet().Include(p => p.CreatedUser).ToList();

            var vm = _mapper.Map<IEnumerable<EmployeeLockReasonViewModel>>(employeeLockReason);

            var response = new BaseResponse<IEnumerable<EmployeeLockReasonViewModel>>(vm, true);
            return response;
        }

        public async Task<BaseResponse<EditEmployeeLockReasonViewModel>> CreateAsync(EditEmployeeLockReasonViewModel employeeLockReasonViewModel)
        {
            var employeeLockReason = _mapper.Map<EmployeeLockReason>(employeeLockReasonViewModel);

            _unitOfWork.EmployeeLockReasons.Add(employeeLockReason);

            await _unitOfWork.CommitAsync();

            var response = BaseResponse<EditEmployeeLockReasonViewModel>.Success(_mapper.Map<EditEmployeeLockReasonViewModel>(employeeLockReason), null);

            return response;
        }

        public async Task<BaseResponse<EditEmployeeLockReasonViewModel>> UpdateAsync(EditEmployeeLockReasonViewModel employeeLockReasonViewModel)
        {
            var employeeLockReason = _mapper.Map<EmployeeLockReason>(employeeLockReasonViewModel);
            _unitOfWork.EmployeeLockReasons.Update(employeeLockReason);

            await _unitOfWork.CommitAsync();

            var response = BaseResponse<EditEmployeeLockReasonViewModel>.Success(_mapper.Map<EditEmployeeLockReasonViewModel>(employeeLockReason), null);

            return response;
        }

        // TODO
        public async Task<BaseResponse<EmployeeLockReasonViewModel>> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
