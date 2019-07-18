using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Account
{
    public class EditUserDetailsViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(128)]
        public string Email { get; set; }
        [StringLength(256)]
        public string FactualAddress { get; set; }
    }
}
