using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Samr.ERP.Infrastructure.Entities
{
    public class User : IdentityUser<Guid>
    {
        public override String Email { get; set;  }

        //Todo need to complete with employee
        public string GetToShortName() => $"{Email}";
    }
}