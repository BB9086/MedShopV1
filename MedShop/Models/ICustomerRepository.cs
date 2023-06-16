using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models
{
   public interface ICustomerRepository
    {
        Customer AddCustomer(Customer customer);
        Customer GetCustomerById(int customerId);
    }
}
