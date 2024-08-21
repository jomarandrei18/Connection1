using Connection1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connection1.Repository
{
    public interface IMenuCategoryRepository
    {
        IQueryable<MenuCategory> GetMenuCategories();
        IQueryable<Product> GetProduct(int Id);
        void AddMenuCategory(List<Order> orders);
        int CountOrderToday();
    }
}
