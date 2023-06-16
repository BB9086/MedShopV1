using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models
{
    [Table("tblCustomers")]
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string LoginUserId { get; set; }

        [Required(ErrorMessage = "Ime je obavezno")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Prezime je obavezno")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Adresa je obavezna")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Poštanksi broj je obavezan")]
        public string ZipCode { get; set; }
        [Required(ErrorMessage = "Ime grada je obavezno")]
        public string City { get; set; }
        [Required(ErrorMessage = "Ime države je obavezno")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Telefonski broj je obavezan")]
        public string TelephoneNumber { get; set; }
        [Required(ErrorMessage = "Email je obavezan")]
        [EmailAddress]
        public string Email { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
