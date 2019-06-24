using System.Collections.Generic;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.ViewModels.Account
{
    public class GetUsersViewModel
    {
        public IEnumerable<User> Users { get; set; }
    }
}
