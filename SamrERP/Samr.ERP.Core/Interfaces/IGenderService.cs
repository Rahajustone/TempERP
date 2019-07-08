using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.Common;

namespace Samr.ERP.Core.Interfaces
{
    public interface IGenderService
    {
        Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllAsync();
    }
}
