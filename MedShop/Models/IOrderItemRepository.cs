using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models
{
   public interface IOrderItemRepository
    {
        OrderItem AddOrderItem(OrderItem orderItem);

    }
}
