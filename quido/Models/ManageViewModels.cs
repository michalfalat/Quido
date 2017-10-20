using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace quido.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required(ErrorMessage = "Vyplnte nové heslo")]
        [StringLength(100, ErrorMessage = " {0} musí míť alespoň {2} znaku.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nové heslo")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potvrďte heslo")]
        [Compare("NewPassword", ErrorMessage = "Hesla se neshodují.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Vyplnte  aktuální heslo")]
        [DataType(DataType.Password)]
        [Display(Name = "Aktuální heslo")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Vyplnte nové heslo")]
        [StringLength(100, ErrorMessage = " {0} musí mít alespoň {2} znaku.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nové heslo")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Zadejte heslo znovu")]
        [Compare("NewPassword", ErrorMessage = "Hesla se neshoduji.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}