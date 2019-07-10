using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
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

        public async Task<BaseDataResponse<PagedList<EditNationalityViewModel>>> GetAllAsync(PagingOptions pagingOptions)
        {
            var query = _unitOfWork
                .Nationalities
                .GetDbSet()
                .Include(u => u.CreatedUser);

            var pageList = await query.ToMappedPagedListAsync<Nationality, EditNationalityViewModel>(pagingOptions);

            return BaseDataResponse<PagedList<EditNationalityViewModel>>.Success(pageList);
        }

        public async Task<BaseDataResponse<EditNationalityViewModel>> GetByIdAsync(Guid id)
        {
            BaseDataResponse<EditNationalityViewModel> dataResponse;

            var nationality = await _unitOfWork.Nationalities.GetDbSet()
                .Include(p => p.CreatedUser)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (nationality == null)
            {
                dataResponse = BaseDataResponse<EditNationalityViewModel>.NotFound(null, new ErrorModel("Not found Entity"));
            }
            else
            {
                dataResponse = BaseDataResponse<EditNationalityViewModel>.Success(_mapper.Map<EditNationalityViewModel>(nationality));
            }

            return dataResponse;
        }

        public async Task<BaseDataResponse<EditNationalityViewModel>> CreateAsync(EditNationalityViewModel editNationalityViewModel)
        {
            BaseDataResponse<EditNationalityViewModel> dataResponse;

            var nationalityExists =
                _unitOfWork.Nationalities
                    .Any(p => p.Name.ToLower() == editNationalityViewModel.Name.ToLower());
            if (nationalityExists)
            {
                dataResponse = BaseDataResponse<EditNationalityViewModel>.Fail(editNationalityViewModel, new ErrorModel("Already this model is in Database"));
            }
            else
            {
                var nationality = _mapper.Map<Nationality>(editNationalityViewModel);
                _unitOfWork.Nationalities.Add(nationality);

                await _unitOfWork.CommitAsync();

                dataResponse = BaseDataResponse<EditNationalityViewModel>.Success(_mapper.Map<EditNationalityViewModel>(nationality));
            }

            return dataResponse;
        }

        public async Task<BaseDataResponse<EditNationalityViewModel>> UpdateAsync(EditNationalityViewModel nationalityViewModel)
        {
            BaseDataResponse<EditNationalityViewModel> dataResponse;

            var nationalityExists = await _unitOfWork.Nationalities.ExistsAsync(nationalityViewModel.Id);
            if (nationalityExists)
            {
                var checkNameUnique = await _unitOfWork.Nationalities
                    .GetDbSet()
                    .AnyAsync(n =>n.Id != nationalityViewModel.Id && n.Name.ToLower() == nationalityViewModel.Name.ToLower());
                if (checkNameUnique)
                {
                    dataResponse = BaseDataResponse<EditNationalityViewModel>.Fail(nationalityViewModel, new ErrorModel("Already we have this nation with this Name"));
                }
                else
                {
                    var nationality = _mapper.Map<Nationality>(nationalityViewModel);

                    _unitOfWork.Nationalities.Update(nationality);
                    await _unitOfWork.CommitAsync();

                    dataResponse = BaseDataResponse<EditNationalityViewModel>.Success(_mapper.Map<EditNationalityViewModel>(nationality));
                }
            }
            else
            {
                dataResponse = BaseDataResponse<EditNationalityViewModel>.NotFound(nationalityViewModel);
            }

            return dataResponse;
        }
    }
}
