using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models.Imp
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly CoreDBContext context;
        public OrderItemRepository(CoreDBContext context)
        {
            this.context = context;
        }
        public OrderItem AddOrderItem(OrderItem orderItem)
        {
            context.OrderItems.Add(orderItem);
            context.SaveChanges();
            return orderItem;
        }

       
    }
}
