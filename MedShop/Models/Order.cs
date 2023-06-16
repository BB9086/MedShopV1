using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models
{
    [Table("tblOrders")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        ////[Key, Column(Order = 1)]
        //public Guid OrderGuid { get; set; }
        public int CustomerId { get; set; }
        public int? StoreId { get; set; }
        public int OrderStatus { get; set; }
        public int ShippingStatus { get; set; }
        public int PaymentStatus { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal OrderTotal { get; set; }
        public string ShippingMethod { get; set; }
        public string CustomerIpAddress { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ShippingDate { get;set; }

        [ForeignKey("StoreId")]
        public Store Store { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
