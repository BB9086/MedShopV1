using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models
{
    [Table("tblCategories")]
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int StoreType { get; set; }
        public ICollection<Product> Products { get; set; }

        [ForeignKey("StoreType")]
        public Store Store { get; set; }
    }
}
