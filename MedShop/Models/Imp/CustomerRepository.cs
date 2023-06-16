using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models.Imp
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CoreDBContext context;
        public CustomerRepository(CoreDBContext context)
        {
            this.context = context;
        }
        public Customer AddCustomer(Customer customer)
        {
            context.Customers.Add(customer);
            context.SaveChanges();
            return customer;
        }

        public Customer GetCustomerById(int customerId)
        {
            return context.Customers.FirstOrDefault(x=>x.CustomerId==customerId);
        }
    }
}
