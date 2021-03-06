﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    public class User : IdentityUser<Guid>
    {
        public Guid EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }

        public override String Email { get; set;  }

        public Guid? UserLockReasonId { get; set; }
        [ForeignKey(nameof(UserLockReasonId))]
        public UserLockReason UserLockReason { get; set; }
        [ForeignKey(nameof(LockUser))]
        public Guid? LockUserId { get; set; }
        public User LockUser { get; set; }
        //Todo need to complete with employee
        public string ToShortName() => $"{UserName}";
        public  DateTime? LockDate { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }

        [MaxLength(4),MinLength(4)]
        public int? ChangePasswordConfirmationCode { get; set; }
        public DateTime? ChangePasswordConfirmationCodeExpires { get; set; }
        
    }
}