using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Infrastructure.Providers
{
    //TODO More info about user
    public class UserProvider
    {
        public User CurrentUser { get; set; }
        public void Initialise(User user)
        {
            CurrentUser = user;
        }
    }
}
