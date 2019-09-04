using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Staff;
using Samr.ERP.Core.ViewModels.EmailSetting;

namespace Samr.ERP.Core.Interfaces
{
    public  interface IEmailMessageHistoryService
    {
        Task<BaseDataResponse<PagedList<EmailMessageHistoryLogViewModel>>> GetAllLogAsync(
            PagingOptions pagingOptions, SortRule sortRule,
            FilterEmailMessageHistoryLogViewModel emailMessageHistoryLogFilterView);
    }
}
