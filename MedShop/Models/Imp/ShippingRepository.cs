using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models.Imp
{
    public class ShippingRepository : IShippingRepository
    {
        private readonly CoreDBContext context;
        public ShippingRepository(CoreDBContext context)
        {
            this.context = context;
        }
        public Shipping AddShippingInfoForNewStore(Shipping shipping)
        {
            context.Shippings.Add(shipping);
            context.SaveChanges();
            return shipping;

        }
    }
}
