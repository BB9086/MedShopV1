using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models.Imp
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CoreDBContext context;
        public OrderRepository(CoreDBContext context)
        {
            this.context = context;
        }
        public Order AddOrder(Order order)
        {
            context.Orders.Add(order);
            context.SaveChanges();
            return order;

        }
    }
}
