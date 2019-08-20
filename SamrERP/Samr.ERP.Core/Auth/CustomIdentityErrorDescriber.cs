using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Samr.ERP.Core.Stuff;

namespace Samr.ERP.Core.Auth
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = ErrorCode.EmailMustBeUnique.ToString(),
                Description = ErrorCode.EmailMustBeUnique.ToStringX()
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = ErrorCode.PhoneMustBeUnique.ToString(),
                Description = ErrorCode.PhoneMustBeUnique.ToStringX()
            };
        }

        //public override IdentityError DuplicateRoleName(string role)
        //{
        //    return new IdentityError
        //    {
        //        Code = nameof(DuplicateRoleName),
        //        Description = string.Format(LocalizedIdentityErrorMessages.DuplicateRoleName, role)
        //    };
        //}
     
    }
}
