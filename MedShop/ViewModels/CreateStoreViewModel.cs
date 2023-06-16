using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.ViewModels
{
    public class CreateStoreViewModel
    {
        [Required(ErrorMessage = "Ime radnje je obavezno")]
        public string StoreName { get; set; }
        [Required(ErrorMessage = "Adresa radnje je obavezna")]
        public string StoreAddress { get; set; }
        [Required(ErrorMessage = "Opis radnje je obavezan")]
        public string StoreDescription { get; set; }
        [Required(ErrorMessage = "Poštanski broj i grad su obavezni")]
        public string ZipCodeAndCity { get; set; }
        [Required(ErrorMessage = "Web site je obavezan")]
        public string WebSite { get; set; }
        [Required(ErrorMessage = "Telefonski broj je obavezan")]
        public string Telephone { get; set; }
        [Required(ErrorMessage = "Slika radnje je obavezna")]
        public IFormFile Photo { get; set; }
        [Required(ErrorMessage = "Vreme čekanja isporuke je obavezno")]
        public int DeliveryShippingMethod { get; set; }
        [Required(ErrorMessage = "Vreme prilikom ličnog preuzimanja je obavezno")]
        public int PickupShippingMethod { get; set; }

        [Required(ErrorMessage = "Email menadžera je obavezan")]
        public string ManagerEmail { get; set; }
    }
}
