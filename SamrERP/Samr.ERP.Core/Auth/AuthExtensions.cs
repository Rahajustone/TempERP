using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Infrastructure.Providers;

namespace Samr.ERP.Core.Auth
{
    public class AuthExtensions
    {
        private readonly UserProvider _userProvider;

        public AuthExtensions(UserProvider userProvider)
        {
            _userProvider = userProvider;
        }

        public bool IsInRole(string role)
        {
            return  _userProvider.ContextUser.IsInRole(role);
        }
    }
}
