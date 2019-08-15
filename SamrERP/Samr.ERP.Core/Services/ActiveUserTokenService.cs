using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class ActiveUserTokenService:IActiveUserTokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;
        private const string ActiveTokenCacheName = "ActiveUsersTokens";
        public ActiveUserTokenService(
            IUnitOfWork unitOfWork,
            IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task AddOrRefreshUserToken(Guid userId, string token)
        {
            var userToken = await _unitOfWork.ActiveUserTokens.All().FirstOrDefaultAsync(p => p.UserId == userId);

            if (userToken == null)
            {
                userToken = new ActiveUserToken()
                {
                    UserId =  userId,
                    Token = token
                };
                _unitOfWork.ActiveUserTokens.Add(userToken);
            }
            else
            {
                userToken.Token = token;
                _unitOfWork.ActiveUserTokens.Update(userToken);
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task<Boolean> TokenActive(string token)
        {
            if (!_cache.TryGetValue(ActiveTokenCacheName,out HashSet<string> activeTokens))
            {
                var tokenExist = await _unitOfWork.ActiveUserTokens.AnyAsync(p => p.Token == token);
                if (tokenExist)
                {
                    if (activeTokens == null) activeTokens = new HashSet<string>();
                    activeTokens.Add(token);
                    _cache.Set(ActiveTokenCacheName, activeTokens,
                        new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
                }
            }
            return activeTokens != null && activeTokens.Contains(token);
        }

        public void DeactivateTokenByUserId(Guid userId)
        {
            var activeToken = _unitOfWork.ActiveUserTokens.All().FirstOrDefault(p => p.UserId == userId);

            if (activeToken != null)
            {
                _unitOfWork.ActiveUserTokens.Delete(activeToken);

                _unitOfWork.CommitAsync(); 

                if (_cache.TryGetValue(ActiveTokenCacheName,out HashSet<string> activeTokens))
                {
                    activeTokens.Remove(activeToken.Token);
                    _cache.Set(ActiveTokenCacheName, activeTokens);
                }
            }
        }
    }
}
