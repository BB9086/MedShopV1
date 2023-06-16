using MedShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShopWebAPi.Data
{
    public interface IMedShopRepo
    {
        IEnumerable<Order> GetAllOrders();

        IEnumerable<Customer> GetAllCustomers();

        IEnumerable<Order> GetAllOrdersForStore(int storeId);

        IEnumerable<Customer> GetAllCustomers(List<int> customerIds);

        Order GetOrderById(int id);

        Customer GetCustomerById(int id);

        IEnumerable<OrderItem> GetOrderItemsByOrderID(int orderId);

        IEnumerable<Product> GetAllProducts();

        Task<IEnumerable<Store>> GetAllStoresForManager(string userName);

        bool IsManagerInDb(string userName/*, string password*/);

        void UpdateOrder(Order order);

    }
}
