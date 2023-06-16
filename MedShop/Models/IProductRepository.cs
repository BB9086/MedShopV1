using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models
{
   public interface IProductRepository
    {
        Product GetProductById(int Id);
        IEnumerable<Product> GetProducts();
        IEnumerable<Product> GetProductsByCategoryType(int categoryType);
        IEnumerable<Product> GetProductsByStoreType(int storeType);
        Product UpdateProduct(Product newProduct);
        Product AddProduct(Product product);
        Product DeleteProduct(int Id);
        Product GetProductByProductCode(string productKey);
    }
}
