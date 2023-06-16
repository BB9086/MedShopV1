using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetCategories();
        IEnumerable<Category> GetCategoriesByStoreType(int storeType);
        Category CreateCategory(Category category);
        Category DeleteCategory(int categoryId);
        Category GetCategoryById(int categoryId);
        Category UpdateCategory(Category newCategory);
        Category AddCategory(Category category);

    }
}
