using System;
using System.Threading.Tasks;

namespace Samr.ERP.Core.Interfaces
{
    public interface IActiveUserTokenService
    {
        Task AddOrRefreshUserToken(Guid userId, string token);
        Task<Boolean> TokenActive(string token);
    }
}