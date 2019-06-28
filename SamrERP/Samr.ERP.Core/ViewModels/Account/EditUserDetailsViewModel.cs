using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Account
{
    public class EditUserDetailsViewModel
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(128)]
        public string Email { get; set; }
        [StringLength(256)]
        public string AddressFact { get; set; }
    }
}
