using System.ComponentModel.DataAnnotations;

namespace Samr.ERP.Core.ViewModels.Account
{
    public class RegisterUserViewModel:NewPasswordViewModel
    {

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Compare(nameof(Phone))]
        public string ConfirmPhone { get; set; }
     
    }
}