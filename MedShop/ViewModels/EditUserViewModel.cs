using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MedShop.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Roles = new List<string>();
            Claims = new List<string>();
        }
        
        public string Id { get; set; }

        [Required(ErrorMessage = "Email je obavezan")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Korisničko ime je obavezno")]
        public string UserName { get; set; }

        public IList<string> Roles { get; set; }

        public IList<string> Claims { get; set; }
    }
}
