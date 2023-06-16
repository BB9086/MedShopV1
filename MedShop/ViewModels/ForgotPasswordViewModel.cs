using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email je obavezan")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
