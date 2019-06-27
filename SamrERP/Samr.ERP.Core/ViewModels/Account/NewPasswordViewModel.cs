using System.ComponentModel.DataAnnotations;

namespace Samr.ERP.Core.ViewModels.Account
{
    public class NewPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}