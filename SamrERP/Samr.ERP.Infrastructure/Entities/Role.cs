using System;
using Microsoft.AspNetCore.Identity;

namespace Samr.ERP.Infrastructure.Entities
{
    public class Role:IdentityRole<Guid>
    {
        public string Description { get; set; }
        public string Category { get; set; }
        public string CategoryName { get; set; }
    }
}