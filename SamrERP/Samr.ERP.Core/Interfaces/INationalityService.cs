using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Handbook.Nationality;

namespace Samr.ERP.Core.Interfaces
{
    public interface INationalityService
    {
        Task<BaseDataResponse<IEnumerable<EditNationalityViewModel>>> GetAllAsync();
        Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllListItemAsync();
        Task<BaseDataResponse<EditNationalityViewModel>> GetByIdAsync(Guid id);
        Task<BaseDataResponse<EditNationalityViewModel>> CreateAsync(EditNationalityViewModel nationalityViewModel);
        Task<BaseDataResponse<EditNationalityViewModel>> UpdateAsync(EditNationalityViewModel nationalityViewModel);
    }
}
