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
using Samr.ERP.Core.ViewModels.Handbook.Nationality;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class NationalityService : INationalityService 
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NationalityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<IEnumerable<EditNationalityViewModel>>> GetAllAsync()
        {
            var nationalities = await _unitOfWork.Nationalities.GetDbSet().Include(u => u.CreatedUser).ToListAsync();
            var vm = _mapper.Map<IEnumerable<EditNationalityViewModel>>(nationalities);

            var response = BaseResponse<IEnumerable<EditNationalityViewModel>>.Success(vm);

            return response;
        }

        public async Task<BaseResponse<EditNationalityViewModel>> GetByIdAsync(Guid id)
        {
            BaseResponse<EditNationalityViewModel> response;

            var nationality = await _unitOfWork.Nationalities.GetDbSet()
                .Include(p => p.CreatedUser)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (nationality == null)
            {
                response = BaseResponse<EditNationalityViewModel>.NotFound(null, new ErrorModel("Not found Entity"));
            }
            else
            {
                response = BaseResponse<EditNationalityViewModel>.Success(_mapper.Map<EditNationalityViewModel>(nationality));
            }

            return response;
        }

        public async Task<BaseResponse<EditNationalityViewModel>> CreateAsync(EditNationalityViewModel editNationalityViewModel)
        {
            BaseResponse<EditNationalityViewModel> response;

            var nationalityExists =
                _unitOfWork.Nationalities
                    .Any(p => p.Name.ToLower() == editNationalityViewModel.Name.ToLower());
            if (nationalityExists)
            {
                response = BaseResponse<EditNationalityViewModel>.Fail(editNationalityViewModel, new ErrorModel("Already this model is in Database"));
            }
            else
            {
                var nationality = _mapper.Map<Nationality>(editNationalityViewModel);
                _unitOfWork.Nationalities.Add(nationality);

                await _unitOfWork.CommitAsync();

                response = BaseResponse<EditNationalityViewModel>.Success(_mapper.Map<EditNationalityViewModel>(nationality));
            }

            return response;
        }

        public async Task<BaseResponse<EditNationalityViewModel>> UpdateAsync(EditNationalityViewModel nationalityViewModel)
        {
            BaseResponse<EditNationalityViewModel> response;

            var nationalityExists = await _unitOfWork.Nationalities.ExistsAsync(nationalityViewModel.Id);
            if (nationalityExists)
            {
                var nationality = _mapper.Map<Nationality>(nationalityViewModel);

                _unitOfWork.Nationalities.Update(nationality);
                await _unitOfWork.CommitAsync();

                response = BaseResponse<EditNationalityViewModel>.Success(_mapper.Map<EditNationalityViewModel>(nationality));
            }
            else
            {
                response = BaseResponse<EditNationalityViewModel>.NotFound(nationalityViewModel);
            }

            return response;
        }
    }
}
