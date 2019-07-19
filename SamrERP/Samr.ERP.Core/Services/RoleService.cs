using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class RoleService:IRoleService
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

        

    }
}
