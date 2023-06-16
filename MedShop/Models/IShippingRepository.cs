using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models
{
    public interface IShippingRepository
    {
        Shipping AddShippingInfoForNewStore(Shipping shipping);
    }
}
