using MedShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShopWebAPi.Dtos
{
    public class OrderItemReadDto : Product
    {
        public int ItemId { get; set; }
        public int OrderId { get; set; }

        //public int ProductId { get; set; }
        public int Quantity { get; set; }

        //[ForeignKey("OrderId")]
        //public Order Order { get; set; }

        //[ForeignKey("ProductId")]
        //public Product Product { get; set; }
    }
}
