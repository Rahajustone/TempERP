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
using Samr.ERP.Core.ViewModels.Common;
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

        private IQueryable<Nationality> GetQuery()
        {
            return _unitOfWork.Nationalities.GetDbSet();
        }

        private IQueryable<Nationality> GetQueryWithUser()
        {
            return GetQuery().Include(u => u.CreatedUser);
        }

        public async Task<BaseDataResponse<IEnumerable<EditNationalityViewModel>>> GetAllAsync()
        {
            var nationalities = await GetQueryWithUser().ToListAsync();

            return BaseDataResponse<IEnumerable<EditNationalityViewModel>>.Success(_mapper.Map<IEnumerable<EditNationalityViewModel>>(nationalities));
        }

        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllListItemAsync()
        {
            var listItems = await GetQuery().Where(n => n.IsActive).ToListAsync();

            return BaseDataResponse<IEnumerable<SelectListItemViewModel>>.Success(_mapper.Map<IEnumerable<SelectListItemViewModel>>(listItems));
        }

        public async Task<BaseDataResponse<EditNationalityViewModel>> GetByIdAsync(Guid id)
        {
            BaseDataResponse<EditNationalityViewModel> dataResponse;

            var nationality = await GetQuery()
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

            var nationalityExists = await GetQuery().FirstOrDefaultAsync(n => n.Id == nationalityViewModel.Id);
            if (nationalityExists != null)
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
                    var nationality = _mapper.Map<EditNationalityViewModel, Nationality>(nationalityViewModel, nationalityExists);

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
