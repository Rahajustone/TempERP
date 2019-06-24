using System.ComponentModel.DataAnnotations;

namespace Samr.ERP.Core.ViewModels.Account
{
    public class RegisterUserViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Compare(nameof(Phone))]
        public string ConfirmPhone { get; set; }

    }
}