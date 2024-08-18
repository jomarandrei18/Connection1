using Connection1.Entities;
using Connection1.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var query = _repository.GetMenuCategories();

            var pagedResult = query.ToList();

            return pagedResult;
        }

        public List<Product> GetProductList(int Id)
        {
            var query = _repository.GetProduct(Id);

            return query.ToList();
        }
    }
}
