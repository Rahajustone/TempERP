using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Infrastructure.Data.Contracts;

namespace Samr.ERP.Core.Services
{
    public class GenderService : IGenderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GenderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllAsync()
        {
            var genders = await _unitOfWork.Genders.GetDbSet().ToListAsync();
            var vm = _mapper.Map<IEnumerable<SelectListItemViewModel>>(genders);

            var response = BaseDataResponse<IEnumerable<SelectListItemViewModel>>.Success(vm);

            return response;
        }
    }
}
