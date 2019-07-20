using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models.ResponseModels;

namespace Samr.ERP.Core.Interfaces
{
    public interface IRoleService
    {
        Task<BaseResponse> AddAsync(string name, string description, string category);
    }
}
