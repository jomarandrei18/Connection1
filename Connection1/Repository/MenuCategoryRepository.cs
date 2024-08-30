using Connection1.Connection;
using Connection1.Entities;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Order = Connection1.Entities.Order;

namespace Connection1.Repository
{
    public class MenuCategoryRepository: IMenuCategoryRepository
    {
        private readonly ConnDbContext _context;

        public MenuCategoryRepository(ConnDbContext context)
        {
            _context = context;
        }

        public IQueryable<MenuCategory> GetMenuCategories()
        {
            return _context.menucategories.AsQueryable();
        }

        public IQueryable<Product> GetProduct(int Id)
        {
            var query =  _context.productlist.AsQueryable();

            return query.Where(c => c.CategId == Id);
        }

        public void AddMenuCategory(List<Entities.Order> orders)
        {
            foreach (var list in orders)
            {
                _context.orders.Add(list);
                _context.SaveChanges();
            }
        }

        public int CountOrderToday()
        {
            DateTime today = DateTime.Today;
            DateTime startOfToday = today;
            DateTime endOfToday = today.AddDays(1).AddTicks(-1);

            return _context.orders.Where(order => order.AddedDate >= startOfToday && order.AddedDate <= endOfToday).Count() + 1;
        }
        public IQueryable<Order> GetOrder()
        {
            return _context.orders;
        }

        public IQueryable<Order> GetOrdersByDateRange(DateTime startDate, DateTime endDate) 
        {
            return _context.orders.Where(o => o.AddedDate >= startDate && o.AddedDate <= endDate);
        }


    }
}
