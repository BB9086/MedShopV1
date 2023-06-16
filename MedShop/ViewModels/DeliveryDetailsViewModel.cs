using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.ViewModels
{
    public class DeliveryDetailsViewModel
    {
        public int CustomerId { get; set; }

        public int StoreId { get; set; }
        
        public string ShippingMethod { get; set; }
        
        public string Contact { get; set; }
       
        public string Address { get; set; }
        
        public string FirstName { get; set; }
       
        public string LastName { get; set; }
        
        public string ZipCode { get; set; }
        
        public string Country { get; set; }
       
        public string City { get; set; }
       
        public string TelephoneNumber { get; set; }
    }
}
