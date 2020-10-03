using stutor_core.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace stutor_core.Repositories
{
    public class DictionaryRepository
    {
        private ApplicationDbContext _context { get; set; }
        private static Random rand;

        public DictionaryRepository(ApplicationDbContext context)
        {
            _context = context;
            rand = new Random();
        }

        public string GetRandomWord()
        {
            var num = rand.Next(1, _context.Dictionary.ToList().Count);
            return _context.Dictionary.OrderBy(r => Guid.NewGuid()).Skip(num - 1).Take(1).First().Word;
        }

    }
}
