using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Dtos
{
    public class OrderUpdateDto
    {
        public int OrderId { get; set; }
        public int StoreId { get; set; }
        public int OrderStatus { get; set; }
        public int ShippingStatus { get; set; }
        public int PaymentStatus { get; set; }
        public decimal OrderTotal { get; set; }
        public string ShippingMethod { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ShippingDate { get; set; }
    }
}
