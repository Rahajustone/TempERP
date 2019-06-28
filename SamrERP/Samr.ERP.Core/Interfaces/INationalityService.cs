using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.Handbook.Nationality;

namespace Samr.ERP.Core.Interfaces
{
    public interface INationalityService
    {
        Task<BaseResponse<IEnumerable<EditNationalityViewModel>>> GetAllAsync();
        Task<BaseResponse<EditNationalityViewModel>> GetByIdAsync(Guid id);
        Task<BaseResponse<EditNationalityViewModel>> CreateAsync(EditNationalityViewModel nationalityViewModel);
        Task<BaseResponse<EditNationalityViewModel>> UpdateAsync(EditNationalityViewModel nationalityViewModel);
    }
}
