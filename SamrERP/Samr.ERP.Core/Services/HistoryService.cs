using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Infrastructure.Data.Contracts;

namespace Samr.ERP.Core.Services
{
    public class HistoryService<TSource, TDest> : IHistoryService<TSource, TDest> where TSource : class where TDest :class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HistoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateHistory(TSource source, TDest dest)
        {
            //var entity = _mapper.Map<TSource>(source);

            _unitOfWork.GetStandardRepo<TSource>().Add(source);

            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}
