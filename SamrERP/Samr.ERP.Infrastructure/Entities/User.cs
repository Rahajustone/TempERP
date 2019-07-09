﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    public class User : IdentityUser<Guid>, IActivable
    {
        public override String Email { get; set;  }

        public Guid? UserLockReasonId { get; set; }
        [ForeignKey(nameof(UserLockReasonId))]
        public UserLockReason UserLockReason { get; set; }

        //Todo need to complete with employee
        public string ToShortName() => $"{UserName}";
       
        public bool IsActive { get; set; }

        public  DateTime LockDate { get; set; }
    }
}