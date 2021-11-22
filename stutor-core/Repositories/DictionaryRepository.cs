using stutor_core.Database;
using System;
using System.Linq;
using stutor_core.Repositories.Interfaces;

namespace stutor_core.Repositories
{
    public class DictionaryRepository : BaseRepository, IDictionaryRepository
    {
        private static Random rand;

        public DictionaryRepository(ApplicationDbContext context) : base(context)
        {
            rand = new Random();
        }

        public string GetRandomWord()
        {
            var num = rand.Next(1, _context.Dictionary.ToList().Count);
            return _context.Dictionary.OrderBy(r => Guid.NewGuid()).Skip(num - 1).Take(1).First().Word;
        }

    }
}
