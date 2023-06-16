using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models.Imp
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CoreDBContext context;
        public CategoryRepository(CoreDBContext context)
        {
            this.context = context;
        }

        public Category AddCategory(Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
            return category;
        }

        public Category CreateCategory(Category category)
        {
            
            context.Categories.Add(category);
            context.SaveChanges();
            return category;
        }

        public Category DeleteCategory(int categoryId)
        {
            Category category = context.Categories.Find(categoryId);
            if (category != null)
            {

                context.Categories.Remove(category);
                context.SaveChanges();
            }

            return category;
        }

        public IEnumerable<Category> GetCategories()
        {
            return context.Categories;
        }

        public IEnumerable<Category> GetCategoriesByStoreType(int storeType)
        {
            return context.Categories.Where(x=>x.StoreType==storeType);
        }

        public Category GetCategoryById(int categoryId)
        {
            Category category = context.Categories.Find(categoryId);
            return category;
        }

        public Category UpdateCategory(Category newCategory)
        {

            var cat = context.Categories.Attach(newCategory);
            cat.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            context.SaveChanges();

            return newCategory;
        }
    }
}
