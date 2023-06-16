using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.ViewModels
{
    public class CreateProductViewModel
    {
        public int StoreType { get; set; }
        [Required(ErrorMessage = "Ime kategorije je obavezno")]
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "Ime proizvoda je obavezno")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Kolicina proizvoda je obavezna")]
        public string QuantityOfProduct { get; set; }
        [Required(ErrorMessage = "Cena proizvoda je obavezna")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Trenutna cena proizvoda je obavezna")]
        public decimal CurrentPrice { get; set; }
        [Required(ErrorMessage = "Ime proizvođača je obavezno")]
        public string Producer { get; set; }
        [Required(ErrorMessage = "Lokacija je obavezna")]
        public string Place { get; set; }
        [Required(ErrorMessage = "Ukupna količina je obavezna")]
        public decimal TotalQuantity_Kg { get; set; }
        [Required(ErrorMessage = "Opis proizvoda je obavezan")]
        public string ProductDescription { get; set; }
        public IFormFile Photo { get; set; }


    }
}
