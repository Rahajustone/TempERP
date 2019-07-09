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

        public async Task<BaseDataResponse<EditEmployeeLockReasonViewModel>> GetByIdAsync(Guid id)
        {
            var employeeLockReason = await _unitOfWork.EmployeeLockReasons.GetDbSet()
                .Include(p => p.CreatedUser)
                .FirstOrDefaultAsync(p => p.Id == id);

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

        public async Task<BaseDataResponse<IEnumerable<EmployeeLockReasonViewModel>>> GetAll()
        {
            var employeeLockReason = await _unitOfWork.EmployeeLockReasons.GetDbSet().Include(p => p.CreatedUser).ToListAsync();
            var vm = _mapper.Map<IEnumerable<EmployeeLockReasonViewModel>>(employeeLockReason);

            var response = BaseDataResponse<IEnumerable<EmployeeLockReasonViewModel>>.Success(vm);

            return response;
        }

        public async Task<BaseDataResponse<EditEmployeeLockReasonViewModel>> CreateAsync(EditEmployeeLockReasonViewModel employeeLockReasonViewModel)
        {
            BaseDataResponse<EditEmployeeLockReasonViewModel> dataResponse;

            var employeeLockReasonExists = _unitOfWork.EmployeeLockReasons.Any(p => p.Name.ToLower() == employeeLockReasonViewModel.Name.ToLower());
            if (employeeLockReasonExists)
            {
                dataResponse = BaseDataResponse<EditEmployeeLockReasonViewModel>.Fail(employeeLockReasonViewModel, new ErrorModel("Already this model in database."));
            }
            else
            {
                var employeeLockReason = _mapper.Map<EmployeeLockReason>(employeeLockReasonViewModel);
                _unitOfWork.EmployeeLockReasons.Add(employeeLockReason);

                await _unitOfWork.CommitAsync();

                dataResponse = BaseDataResponse<EditEmployeeLockReasonViewModel>.Success(_mapper.Map<EditEmployeeLockReasonViewModel>(employeeLockReason));
            }

            return dataResponse;
        }

        public async Task<BaseDataResponse<EditEmployeeLockReasonViewModel>> UpdateAsync(EditEmployeeLockReasonViewModel employeeLockReasonViewModel)
        {
            BaseDataResponse<EditEmployeeLockReasonViewModel> dataResponse;

            var employeeLockReasonExists = await _unitOfWork.EmployeeLockReasons.ExistsAsync(employeeLockReasonViewModel.Id);
            if (employeeLockReasonExists)
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
                    var employeeLockReason = _mapper.Map<EmployeeLockReason>(employeeLockReasonViewModel);

                    _unitOfWork.EmployeeLockReasons.Update(employeeLockReason);

                    await _unitOfWork.CommitAsync();

                    dataResponse = BaseDataResponse<EditEmployeeLockReasonViewModel>.Success(_mapper.Map<EditEmployeeLockReasonViewModel>(employeeLockReason));
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
