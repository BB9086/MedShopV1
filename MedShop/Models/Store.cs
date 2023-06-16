using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models
{[Table("tblStores")]
    public class Store
    {
        [Key]
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public Guid StoreGuid { get; set; }
        public string StoreAddress { get; set; }
        public string StoreDescription { get; set; }
        public string ZipCodeAndCity { get; set; }
        public string WebSite { get; set; }
        public string Telephone { get; set; }
        public string ImageSource { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public string CreatedByUserId { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<OpeningHours> OpeningHourss { get; set; }
        public ICollection<Shipping> Shippings { get; set; }
    }
}
