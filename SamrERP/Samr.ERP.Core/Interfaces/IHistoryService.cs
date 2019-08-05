using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.AutoMapper.AutoMapperProfiles;

namespace Samr.ERP.Core.Interfaces
{
    public interface IHistoryService<in TSource, in TDest> where TSource :class where TDest : class
    {
        Task<bool> CreateHistory(TSource source, TDest dest);
    }
}
