using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Interfaces
{
    public interface IUserService
    {
        Task<BaseResponse<User>> CreateAsync(User user, string password);
        Task<User> GetByUserName(string userName);
        Task<User> GetByPhoneNumber(string phoneNumber);
        Task<User> GetCurrentUserAsync(ClaimsPrincipal userPrincipal);
        IEnumerable<User> GetAllUser();
    }
}