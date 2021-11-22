using stutor_core.Models.Sql;
using System.Collections.Generic;
using stutor_core.Services.Interfaces;
using stutor_core.Repositories.Interfaces;

namespace stutor_core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
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
