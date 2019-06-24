using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.WebApi.ViewModels.Account
{
    public class GetUsersViewModel
    {
        public IEnumerable<User> Users { get; set; }
    }
}
