using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models
{
    public interface IStoreRepository
    {
        IEnumerable<Store> GetStores();

        Store GetStoreById(int storeId);

        int GetBufferTimeByStoreTypeAndShippingMethod(int storeId, string shippingMethod);
        Store InsertStore(Store store);

        Store GetStoreByCreatedManagerId(string managerId);

        Store UpdateStore(Store store);

        Store DeleteStore(int storeId);

    }
}
