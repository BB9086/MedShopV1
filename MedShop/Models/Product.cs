using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models
{   [Table("tblProducts")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public string DescriptionId { get; set; }

        [NotMapped]
        public int NumberOfSameProductInShoppingCart { get; set; }

        public string QuantityOfProduct { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentPrice { get; set; }
        public string Producer { get; set; }
        public string Place { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalQuantity_Kg { get; set; }
        public string ProductDescription { get; set; }
        public string ProductKey { get; set; }
        public int? CategoryType { get; set; }
        public int? StoreType { get; set; }
        public string ImageSource { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }

        [ForeignKey("CategoryType")]
        public Category Category { get; set; }

        [ForeignKey("StoreType")]
        public Store Store { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

    }
}
