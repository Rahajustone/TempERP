using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class ActiveUserTokenService:IActiveUserTokenService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ActiveUserTokenService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            return await _unitOfWork.ActiveUserTokens.AnyAsync(p => p.Token == token);
        }

        public void DeactivateTokenByUserId(Guid userId)
        {
            var activeToken = _unitOfWork.ActiveUserTokens.All().FirstOrDefault(p => p.UserId == userId);

            if (activeToken != null)
            {
                _unitOfWork.ActiveUserTokens.Delete(activeToken);

                _unitOfWork.CommitAsync(); 
            }
        }
    }
}
