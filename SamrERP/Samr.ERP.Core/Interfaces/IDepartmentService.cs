using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Interfaces
{
    public interface IDepartmentService
    {
        Task<BaseResponse<Department>> GetByIdAsync(Guid id);
        Task<BaseResponse<IEnumerable<Department>>> GetAll();
        Task<BaseResponse<Department>> CreateAsync(Department employee);
        //IEnumerable<Department> GetAllUser();
    }
}
