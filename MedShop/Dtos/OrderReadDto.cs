using MedShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShopWebAPi.Dtos
{
    public class OrderReadDto : Customer
    {
   
        public int OrderId { get; set; }

        //public int CustomerId { get; set; }

        public int StoreId { get; set; }
        public int OrderStatus { get; set; }
        public int ShippingStatus { get; set; }
        public int PaymentStatus { get; set; }
        public decimal OrderTotal { get; set; }
        public string ShippingMethod { get; set; }
        //public string CustomerIpAddress { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ShippingDate { get; set; }

        //[ForeignKey("StoreId")]
        //public Store Store { get; set; }

        //[ForeignKey("CustomerId")]
        //public Customer Customer { get; set; }
        //public ICollection<OrderItem> OrderItems { get; set; }
    }
}
