using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<Role> _roleManager;
        private readonly ActiveUserTokenService _activeUserTokenService;

        public RoleService(
            IUnitOfWork unitOfWork,
            RoleManager<Role> roleManager
            )
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }

        public async Task<BaseResponse> AddAsync(Role role)
        {

            var roleAddResult = await _roleManager.CreateAsync(role);
            return !roleAddResult.Succeeded
                ? BaseResponse.Fail(roleAddResult.Errors.ToErrorModels())
                : BaseResponse.Success();
        }
        

    }
}
