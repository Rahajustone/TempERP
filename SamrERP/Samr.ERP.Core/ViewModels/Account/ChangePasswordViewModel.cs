using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Account
{
    public class ChangePasswordViewModel:NewPasswordViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [Range(1000,9999)]
        public int SmsConfirmationCode { get; set; }
    }
}
