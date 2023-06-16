using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.ViewModels
{
    public class EditProductViewModel
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }
        [Required]
        public string QuantityOfProduct { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int StoreType { get; set; }
        [Required]
        public int CategoryType { get; set; }
        [Required]
        public string ProductKey { get; set; }

        [Required]
        public decimal CurrentPrice { get; set; }
        [Required]
        public string Producer { get; set; }
        [Required]
        public string Place { get; set; }

        [Required]
        public decimal TotalQuantity_Kg { get; set; }

        [Required]
        public string ProductDescription { get; set; }

        public string ImageSource { get; set; }
        
        public IFormFile Photo { get; set; }
    }
}
