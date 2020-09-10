using stutor_core.Database;
using stutor_core.Models.Sql;
using System.Collections.Generic;
using System.Linq;

namespace stutor_core.Repositories
{
    public class CategoryRepository
    {
        private ApplicationDbContext _context { get; set; }

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
