using Connection1.Entities;
using Connection1.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace Connection1.Service
{
    public class MenuCategoryService : IMenuCategoryService
    {
        private readonly IMenuCategoryRepository _repository;

        public MenuCategoryService(IMenuCategoryRepository repository)
        {
            _repository = repository;
        }

        public List<MenuCategory> GetPagedMenuCategories()
        {
            return _repository.GetMenuCategories().ToList();
        }

        public List<Product> GetProductList(int Id)
        {
            return _repository.GetProduct(Id).ToList();
        }

        public void AddMenuCategory(List<Order> orders)
        {
            _repository.AddMenuCategory(orders);
        }

        public int CountOrderToday()
        {
            return _repository.CountOrderToday();
        }

        public IEnumerable<OrderSummary> GetOrder()
        {
            DateTime today = DateTime.Today;
            DateTime endOfToday = today.AddDays(1).AddTicks(-1);

            var orders = _repository.GetOrdersByDateRange(today, endOfToday);
            return orders
                .AsEnumerable()
                .GroupBy(p => p.AddedDate.Date)
                .Select(g => new OrderSummary
                {
                    OrderDate = g.Key,
                    TotalPrice = g.Sum(o => o.TotalPrice)
                })
                .ToList();
        }

        public IEnumerable<OrderSummary> GetOrdersForTodayAndYesterday()
        {
            DateTime today = DateTime.Today;
            DateTime yesterday = today.AddDays(-1);

            var orders = _repository.GetOrdersByDateRange(yesterday, today);

            return orders.
                AsEnumerable()
                .GroupBy(o => o.AddedDate.Date)
                .Select(g => new OrderSummary
                {
                    OrderDate = g.Key,
                    TotalPrice = g.Sum(o => o.TotalPrice)
                })
                .ToList();


        }


    }
}
