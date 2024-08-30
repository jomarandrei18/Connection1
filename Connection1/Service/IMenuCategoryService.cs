using Connection1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connection1.Service
{
    public interface IMenuCategoryService
    {
        List<MenuCategory> GetPagedMenuCategories();
        List<Product> GetProductList(int Id);
        void AddMenuCategory(List<Order> orders);
        int CountOrderToday();
        IEnumerable<OrderSummary> GetOrder();
    }
}
