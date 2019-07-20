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

        public RoleService(
            IUnitOfWork unitOfWork,
            RoleManager<Role> roleManager
            )
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }

        public async Task<BaseResponse> AddAsync(string name, string description, string category)
        {
            var role = new Role()
            {
                Name = name,
                Description = description,
                Category = category
            };

            var roleAddResult = await _roleManager.CreateAsync(role);
            return !roleAddResult.Succeeded
                ? BaseResponse.Fail(roleAddResult.Errors.ToErrorModels())
                : BaseResponse.Success();
        }



    }
}
