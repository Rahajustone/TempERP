namespace Samr.ERP.Core.ViewModels.Account
{
    public class LoginViewModel
    {
        // TODO Raha Changed Username To Phone number for Auth
        //public string UserName { get; set; }
        public bool RememberMe { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
