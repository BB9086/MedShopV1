using Microsoft.AspNetCore.Identity;
using MedShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShopWebAPi.Data
{
    public class SqlMedShopRepo : IMedShopRepo
    {
        private readonly CoreDBContext _medShopContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public SqlMedShopRepo(CoreDBContext medShopContext, UserManager<ApplicationUser> userManager)
        {
            _medShopContext = medShopContext;
            _userManager = userManager;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _medShopContext.Orders.ToList();
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _medShopContext.Customers.ToList();
        }

        public IEnumerable<Order> GetAllOrdersForStore(int storeId)
        {
            return _medShopContext.Orders.Where(x => x.StoreId == storeId);
        }

        public IEnumerable<Customer> GetAllCustomers(List<int> customerIds)
        {
            return _medShopContext.Customers.Where(x => customerIds.Contains(x.CustomerId));
        }

        public Order GetOrderById(int orderId)
        {
            return _medShopContext.Orders.FirstOrDefault(x => x.OrderId == orderId);
        }

        public Customer GetCustomerById(int customerId)
        {
            return _medShopContext.Customers.FirstOrDefault(x => x.CustomerId == customerId);
        }

        public IEnumerable<OrderItem> GetOrderItemsByOrderID(int orderId)
        {
            return _medShopContext.OrderItems.Where(x => x.OrderId == orderId).ToList();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _medShopContext.Products.ToList();
        }

        public async Task<IEnumerable<Store>> GetAllStoresForManager(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var id = user.Id;
            var stores = _medShopContext.Stores.Where(x => x.CreatedByUserId == id);
            return _medShopContext.Stores.Where(x => x.CreatedByUserId == id);
        }


        public bool IsManagerInDb(string userName/*, string password*/)
        {
            var userManager = _userManager.Users.FirstOrDefault(x=> x.UserName == userName /*&& x.PasswordHash == password.GetHashCode().ToString()*/);

            if (userManager != null)
            {
                return true;
            }
            else
            {
                return false;
            }
           
        }

        public void UpdateOrder(Order order)
        {
            _medShopContext.SaveChanges();
        }
    }
}
