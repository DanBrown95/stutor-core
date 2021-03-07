using stutor_core.Database;
using stutor_core.Models.Sql;
using stutor_core.Repositories;
using System.Collections.Generic;

namespace stutor_core.Services
{
    public class CategoryService
    {
        private readonly CategoryRepository _repo;

        public CategoryService(ApplicationDbContext context)
        {
            _repo = new CategoryRepository(context);
        }

        public Category Get(int id)
        {
            return _repo.Get(id);
        }

        public IEnumerable<Category> GetAll()
        {
            return _repo.GetAll();
        }

        public void Add(Category category)
        {
            _repo.Add(category);
        }

        public IEnumerable<PopularCategory> GetAllPopular()
        {
            return _repo.GetAllPopular();
        }

    }
}
