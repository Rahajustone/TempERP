using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.Handbook;

namespace Samr.ERP.Core.Interfaces
{
    public interface IHandbookService
    {
        Task<BaseDataResponse<IEnumerable<HandbookViewModel>>> GetAllAsync();
        Task<bool> ChangeStatus(string name, Guid id);
    }
}
