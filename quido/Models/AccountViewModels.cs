using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace quido.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
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
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class SexList
    {
        public static Dictionary<string, string> sex { get; } = new Dictionary<string, string>()
        {
              {"male" , Resources.Resource.Male},
              {"female" , Resources.Resource.Female},
        };

    }
    public class Years
    {
        public static Dictionary<int, string> years { get; } = new Dictionary<int, string>()
        {
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


        };

    }

    public class Countries
    {
        public static Dictionary<string, string> countries { get; } = new Dictionary<string, string>()
        {
              {"afghan" , "Afghan"},
              {"albanian" , "Albanian"},
              {"algerian" , "Algerian"},
              {"american" , "American"},
              {"andorran" , "Andorran"},
              {"angolan" , "Angolan"},
              {"antiguans" , "Antiguans"},
              {"argentinean" , "Argentinean"},
              {"armenian" , "Armenian"},
              {"australian" , "Australian"},
              {"austrian" , "Austrian"},
              {"azerbaijani" , "Azerbaijani"},
              {"bahamian" , "Bahamian"},
              {"bahraini" , "Bahraini"},
              {"bangladeshi" , "Bangladeshi"},
              {"barbadian" , "Barbadian"},
              {"barbudans" , "Barbudans"},
              {"batswana" , "Batswana"},
              {"belarusian" , "Belarusian"},
              {"belgian" , "Belgian"},
              {"belizean" , "Belizean"},
              {"beninese" , "Beninese"},
              {"bhutanese" , "Bhutanese"},
              {"bolivian" , "Bolivian"},
              {"bosnian" , "Bosnian"},
              {"brazilian" , "Brazilian"},
              {"british" , "British"},
              {"bruneian" , "Bruneian"},
              {"bulgarian" , "Bulgarian"},
              {"burkinabe" , "Burkinabe"},
              {"burmese" , "Burmese"},
              {"burundian" , "Burundian"},
              {"cambodian" , "Cambodian"},
              {"cameroonian" , "Cameroonian"},
              {"canadian" , "Canadian"},
              {"cape verdean" , "Cape Verdean"},
              {"central african" , "Central African"},
              {"chadian" , "Chadian"},
              {"chilean" , "Chilean"},
              {"chinese" , "Chinese"},
              {"colombian" , "Colombian"},
              {"comoran" , "Comoran"},
              {"congolese" , "Congolese"},
              {"costa rican" , "Costa Rican"},
              {"croatian" , "Croatian"},
              {"cuban" , "Cuban"},
              {"cypriot" , "Cypriot"},
              {"czech" , "Czech"},
              {"danish" , "Danish"},
              {"djibouti" , "Djibouti"},
              {"dominican" , "Dominican"},
              {"dutch" , "Dutch"},
              {"east timorese" , "East Timorese"},
              {"ecuadorean" , "Ecuadorean"},
              {"egyptian" , "Egyptian"},
              {"emirian" , "Emirian"},
              {"equatorial guinean" , "Equatorial Guinean"},
              {"eritrean" , "Eritrean"},
              {"estonian" , "Estonian"},
              {"ethiopian" , "Ethiopian"},
              {"fijian" , "Fijian"},
              {"filipino" , "Filipino"},
              {"finnish" , "Finnish"},
              {"french" , "French"},
              {"gabonese" , "Gabonese"},
              {"gambian" , "Gambian"},
              {"georgian" , "Georgian"},
              {"german" , "German"},
              {"ghanaian" , "Ghanaian"},
              {"greek" , "Greek"},
              {"grenadian" , "Grenadian"},
              {"guatemalan" , "Guatemalan"},
              {"guinea-bissauan" , "Guinea-Bissauan"},
              {"guinean" , "Guinean"},
              {"guyanese" , "Guyanese"},
              {"haitian" , "Haitian"},
              {"herzegovinian" , "Herzegovinian"},
              {"honduran" , "Honduran"},
              {"hungarian" , "Hungarian"},
              {"icelander" , "Icelander"},
              {"indian" , "Indian"},
              {"indonesian" , "Indonesian"},
              {"iranian" , "Iranian"},
              {"iraqi" , "Iraqi"},
              {"irish" , "Irish"},
              {"israeli" , "Israeli"},
              {"italian" , "Italian"},
              {"ivorian" , "Ivorian"},
              {"jamaican" , "Jamaican"},
              {"japanese" , "Japanese"},
              {"jordanian" , "Jordanian"},
              {"kazakhstani" , "Kazakhstani"},
              {"kenyan" , "Kenyan"},
              {"kittian and nevisian" , "Kittian and Nevisian"},
              {"kuwaiti" , "Kuwaiti"},
              {"kyrgyz" , "Kyrgyz"},
              {"laotian" , "Laotian"},
              {"latvian" , "Latvian"},
              {"lebanese" , "Lebanese"},
              {"liberian" , "Liberian"},
              {"libyan" , "Libyan"},
              {"liechtensteiner" , "Liechtensteiner"},
              {"lithuanian" , "Lithuanian"},
              {"luxembourger" , "Luxembourger"},
              {"macedonian" , "Macedonian"},
              {"malagasy" , "Malagasy"},
              {"malawian" , "Malawian"},
              {"malaysian" , "Malaysian"},
              {"maldivan" , "Maldivan"},
              {"malian" , "Malian"},
              {"maltese" , "Maltese"},
              {"marshallese" , "Marshallese"},
              {"mauritanian" , "Mauritanian"},
              {"mauritian" , "Mauritian"},
              {"mexican" , "Mexican"},
              {"micronesian" , "Micronesian"},
              {"moldovan" , "Moldovan"},
              {"monacan" , "Monacan"},
              {"mongolian" , "Mongolian"},
              {"moroccan" , "Moroccan"},
              {"mosotho" , "Mosotho"},
              {"motswana" , "Motswana"},
              {"mozambican" , "Mozambican"},
              {"namibian" , "Namibian"},
              {"nauruan" , "Nauruan"},
              {"nepalese" , "Nepalese"},
              {"new zealander" , "New Zealander"},
              {"ni-vanuatu" , "Ni-Vanuatu"},
              {"nicaraguan" , "Nicaraguan"},
              {"nigerien" , "Nigerien"},
              {"north korean" , "North Korean"},
              {"northern irish" , "Northern Irish"},
              {"norwegian" , "Norwegian"},
              {"omani" , "Omani"},
              {"pakistani" , "Pakistani"},
              {"palauan" , "Palauan"},
              {"panamanian" , "Panamanian"},
              {"papua new guinean" , "Papua New Guinean"},
              {"paraguayan" , "Paraguayan"},
              {"peruvian" , "Peruvian"},
              {"polish" , "Polish"},
              {"portuguese" , "Portuguese"},
              {"qatari" , "Qatari"},
              {"romanian" , "Romanian"},
              {"russian" , "Russian"},
              {"rwandan" , "Rwandan"},
              {"saint lucian" , "Saint Lucian"},
              {"salvadoran" , "Salvadoran"},
              {"samoan" , "Samoan"},
              {"san marinese" , "San Marinese"},
              {"sao tomean" , "Sao Tomean"},
              {"saudi" , "Saudi"},
              {"scottish" , "Scottish"},
              {"senegalese" , "Senegalese"},
              {"serbian" , "Serbian"},
              {"seychellois" , "Seychellois"},
              {"sierra leonean" , "Sierra Leonean"},
              {"singaporean" , "Singaporean"},
              {"slovakian" , "Slovakian"},
              {"slovenian" , "Slovenian"},
              {"solomon islander" , "Solomon Islander"},
              {"somali" , "Somali"},
              {"south african" , "South African"},
              {"south korean" , "South Korean"},
              {"spanish" , "Spanish"},
              {"sri lankan" , "Sri Lankan"},
              {"sudanese" , "Sudanese"},
              {"surinamer" , "Surinamer"},
              {"swazi" , "Swazi"},
              {"swedish" , "Swedish"},
              {"swiss" , "Swiss"},
              {"syrian" , "Syrian"},
              {"taiwanese" , "Taiwanese"},
              {"tajik" , "Tajik"},
              {"tanzanian" , "Tanzanian"},
              {"thai" , "Thai"},
              {"togolese" , "Togolese"},
              {"tongan" , "Tongan"},
              {"trinidadian or tobagonian" , "Trinidadian or Tobagonian"},
              {"tunisian" , "Tunisian"},
              {"turkish" , "Turkish"},
              {"tuvaluan" , "Tuvaluan"},
              {"ugandan" , "Ugandan"},
              {"ukrainian" , "Ukrainian"},
              {"uruguayan" , "Uruguayan"},
              {"uzbekistani" , "Uzbekistani"},
              {"venezuelan" , "Venezuelan"},
              {"vietnamese" , "Vietnamese"},
              {"welsh" , "Welsh"},
              {"yemenite" , "Yemenite"},
              {"zambian" , "Zambian"},
              {"zimbabwean" , "Zimbabwean"},
        };

    }

    public class RegisterViewModel
    {
        
        public Dictionary<string, string> myCountries = Countries.countries;
        public Dictionary<int, string> myYears = Years.years;
        public Dictionary<string, string> mySex = SexList.sex;

        [Required]
        [Display(ResourceType = typeof(Resources.Resource), Name = "Username")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.Resource), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.Resource), Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources.Resource), Name = "Nationality")]
        public string Nationality { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources.Resource), Name = "Sex")]
        public string Sex { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources.Resource), Name = "YearOfBirth")]
        public string YearOfBirth { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
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
