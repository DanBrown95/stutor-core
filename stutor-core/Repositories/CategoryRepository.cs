using Microsoft.EntityFrameworkCore;
using stutor_core.Database;
using stutor_core.Models.Sql;
using System.Collections.Generic;
using System.Linq;
using stutor_core.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace stutor_core.Repositories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context, IConfiguration config) : base(context, config) { }

        public void Add(Category cat)
        {
            _context.Add(cat);
            _context.SaveChanges();
        }

        public Category Get(int ID)
        {
            return _context.Category.FirstOrDefault(e => e.Id == ID);
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Category.ToList<Category>();
        }

        public IEnumerable<PopularCategory> GetAllPopular()
        {
            return _context.PopularCategory.Include(x => x.Category).ToList();
        }
    }
}
