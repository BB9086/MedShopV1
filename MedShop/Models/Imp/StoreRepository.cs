using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models.Imp
{
    public class StoreRepository : IStoreRepository
    {
        private readonly CoreDBContext context;

        public StoreRepository(CoreDBContext context)
        {
            this.context = context;
        }

        public Store DeleteStore(int storeId)
        {
            Store store = context.Stores.Find(storeId);
            if (store != null)
            {

                context.Stores.Remove(store);
                context.SaveChanges();
            }

            return store;
        }

       

        public int GetBufferTimeByStoreTypeAndShippingMethod(int storeId, string shippingMethod)
        {
            return context.Shippings.FirstOrDefault(x => x.StoreType == storeId && x.ShippingMethod == shippingMethod).BufferTime;
        }

        public Store GetStoreByCreatedManagerId(string managerId)
        {
            return context.Stores.FirstOrDefault(x => x.CreatedByUserId == managerId);
        }

        public Store GetStoreById(int storeId)
        {
            return context.Stores.FirstOrDefault(x => x.StoreId == storeId);
        }

        public IEnumerable<Store> GetStores()
        {
            return context.Stores;
        }

        public Store InsertStore(Store store)
        {
            context.Stores.Add(store);
            context.SaveChanges();
            return store;
        }

        public Store UpdateStore(Store store)
        {
            var storeChanged = context.Stores.Attach(store);
            storeChanged.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            context.SaveChanges();

            return store;
        }
    }
}
