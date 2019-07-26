using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Department;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Interfaces
{
    public interface IDepartmentService
    {
        Task<BaseDataResponse<EditDepartmentViewModel>> GetByIdAsync(Guid id);
        Task<BaseDataResponse<PagedList<EditDepartmentViewModel>>> GetAllAsync(PagingOptions pagingOptions, FilterHandbookViewModel filterHandbook, SortRule sortRule);
        Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> GetAllSelectListItemAsync();
        Task<BaseDataResponse<EditDepartmentViewModel>> CreateAsync(EditDepartmentViewModel departmentViewModel);
        Task<BaseDataResponse<EditDepartmentViewModel>> UpdateAsync(EditDepartmentViewModel departmentViewModel);
    }
}
