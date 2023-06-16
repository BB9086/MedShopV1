using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models
{
    [Table("tblShippingMethod")]
    public class Shipping
    {
        [Key]
        public int Id { get; set; }
        public string ShippingMethod { get; set; }
        public int BufferTime { get; set; }
        public int StoreType { get; set; }

        [ForeignKey("StoreType")]
        public Store Store { get; set; }
    }
}
