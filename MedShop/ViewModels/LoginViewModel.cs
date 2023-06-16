using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Korisničko ime je obavezno")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        
        [Display(Name = "Zapamti me")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
        public List<AuthenticationScheme> ExternalLogins { get; set; }
    }
}
