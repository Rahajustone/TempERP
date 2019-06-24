using System.ComponentModel.DataAnnotations;

namespace Samr.ERP.Core.ViewModels.Account
{
    public class ResetPasswordViewModel
    {
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}
