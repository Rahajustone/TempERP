﻿using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Samr.ERP.Infrastructure.Entities
{
    public class User : IdentityUser<Guid>
    {
        [StringLength(maximumLength:100,ErrorMessage = "Address length must be not more 100")]
        public String Address { get; set; }
        
    }
}