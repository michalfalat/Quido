using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace quido.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage = "Vyplnte Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Kód")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Zapamatovat?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required(ErrorMessage = "Vyplnte Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        //[Required]
        //[Display(Name = "Email")]
        //[EmailAddress]
        //public string Email { get; set; }

        [Required(ErrorMessage = "Vyplnte přihl. jméno")]
        [Display(Name = "Přihl. jméno")]        
        public string UserName { get; set; }

        [Required(ErrorMessage = "Vyplnte heslo")]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password { get; set; }

        [Display(Name = "Zapamatovat?")]
        public bool RememberMe { get; set; }
    }

    public class SexList
    {
        public static Dictionary<string, string> sex { get; } = new Dictionary<string, string>()
        {
              {"male" , "Muž"},
              {"female" , "Žena"},
        };

    }
    public class Years
    {
        public static Dictionary<int, string> years { get; } = new Dictionary<int, string>()
        {
              {1971 , "1971"},
              {1972 , "1972"},
              {1973 , "1973"},
              {1974 , "1974"},
              {1975 , "1975"},
              {1976 , "1976"},
              {1977 , "1977"},
              {1978 , "1978"},
              {1979 , "1979"},
              {1980 , "1980"},
              {1981 , "1981"},
              {1982 , "1982"},
              {1983 , "1983"},
              {1984 , "1984"},
              {1985 , "1985"},
              {1986 , "1986"},
              {1987 , "1987"},
              {1988 , "1988"},
              {1989 , "1989"},
              {1990 , "1990"},
              {1991 , "1991"},
              {1992 , "1992"},
              {1993 , "1993"},
              {1994 , "1994"},
              {1995 , "1995"},
              {1996 , "1996"},
              {1997 , "1997"},
              {1998 , "1998"},
              {1999 , "1999"},
              {2000 , "2000"},
              {2001 , "2001"},
              {2002 , "2002"},
              {2003 , "2003"},
              {2004 , "2004"},
              {2005 , "2005"},
              {2006 , "2006"},
              {2007 , "2007"},
              {2008 , "2008"},
              {2009 , "2009"},
              {2010 , "2010"},
              {2011 , "2011"},
              {2012 , "2012"},
              {2013 , "2013"},
              {2014 , "2014"},
              {2015 , "2015"},
              {2016 , "2016"},


        };

    }

    public class Countries
    {
        public static Dictionary<string, string> countries { get; } = new Dictionary<string, string>()
        {
              
              {"SK" , "Slovensko"},
              {"CZ" , "Česko"},
              {"undefined" , "Jiné.."},
        };

    }

    public class RegisterViewModel
    {
        
        public Dictionary<string, string> myCountries = Countries.countries;
        public Dictionary<int, string> myYears = Years.years;
        public Dictionary<string, string> mySex = SexList.sex;

        [Required(ErrorMessage = "Vyplnte jméno")]
        [Display(Name = "Přihl. jméno")]
        public string Username { get; set; }

        [Required(ErrorMessage ="Vyplnte e-mail")]
        [EmailAddress(ErrorMessage = "Vyplnte e-mail správně")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vyplnte heslo")]
        [StringLength(100, ErrorMessage = "{0} Musí mít nejméne {2} znaku.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display( Name = "Heslo")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display( Name = "Heslo znovu")]
        [Compare("Password", ErrorMessage = "Hesla se neshodují.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display( Name = "Stát")]
        public string Nationality { get; set; }

        [Required]
        [Display(Name = "Pohlaví")]
        public string Sex { get; set; }

        [Required]
        [Display( Name = "Rok narození")]
        public string YearOfBirth { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Vyplnte e-mail")]
        [EmailAddress(ErrorMessage = "Email neni správnej")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vyplnte heslo")]
        [StringLength(100, ErrorMessage = "{0} Musí mít nejméne {2} znaku.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potvrď heslo")]
        [Compare("Password", ErrorMessage = "Hesla se neshodují.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
