using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models.Imp
{
    public class ProductRepository : IProductRepository
    {
        private readonly CoreDBContext context;

        public ProductRepository(CoreDBContext context)
        {
            this.context = context;
        }

        public Product AddProduct(Product product)
        {
            context.Products.Add(product);
            context.SaveChanges();
            return product;
        }

        public Product DeleteProduct(int Id)
        {
            Product product= context.Products.Find(Id);
            if(product != null)
            {
                
                context.Products.Remove(product);
                context.SaveChanges();
            }

            return product;
           
        }

        public Product GetProductById(int Id)
        {
            Product product= context.Products.Find(Id);
            return product;
        }

        public Product GetProductByProductCode(string productKey)
        {
            Product product = context.Products.FirstOrDefault(x => x.ProductKey == productKey);
            return product;
        }

        public IEnumerable<Product> GetProducts()
        {
            return context.Products;
        }

        public IEnumerable<Product> GetProductsByCategoryType(int categoryType)
        {
            return context.Products.Where(x=>x.CategoryType==categoryType);
        }

        public IEnumerable<Product> GetProductsByStoreType(int storeType)
        {
            return context.Products.Where(x=>x.StoreType==storeType);
        }

        public Product UpdateProduct(Product newProduct)
        {
              
            var prod= context.Products.Attach(newProduct);
            prod.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            context.SaveChanges();

            return newProduct;
        }
    }
}
