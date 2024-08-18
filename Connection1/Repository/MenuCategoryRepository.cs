using Connection1.Connection;
using Connection1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
