using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.News;

namespace Samr.ERP.Core.Interfaces
{
    public interface INewsService
    {
        Task<BaseDataResponse<EditNewsViewModel>> GetByIdAsync(Guid id);
        Task<BaseDataResponse<PagedList<EditNewsViewModel>>> GetAllAsync(PagingOptions pagingOptions);
        Task<BaseDataResponse<EditNewsViewModel>> CreateAsync(EditNewsViewModel newsViewModel);
        Task<BaseDataResponse<EditNewsViewModel>> UpdateAsync(EditNewsViewModel newsViewModel);
    }
}
