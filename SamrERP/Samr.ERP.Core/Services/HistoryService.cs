using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities.BaseObjects;

namespace Samr.ERP.Core.Services
{
    public class HistoryService<TSource, TDest> : IHistoryService<TSource, TDest> where TDest : class where TSource : class, IBaseObject
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

            var id = ((IBaseObject) source).Id;
            _unitOfWork.GetStandardRepo<TDest>().Add(dest);

            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}
