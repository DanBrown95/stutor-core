using stutor_core.Database;
using stutor_core.Models.Sql;
using System.Collections.Generic;
using System.Linq;
using stutor_core.Repositories.Interfaces;

namespace stutor_core.Repositories
{
    public class TimezoneRepository : BaseRepository, ITimezoneRepository
    {
        public TimezoneRepository(ApplicationDbContext context) : base(context) { }

        public void Add(Timezone timezone)
        {
            _context.Add(timezone);
            _context.SaveChanges();
        }

        public Timezone Get(int ID)
        {
            return _context.Timezone.FirstOrDefault(e => e.Id == ID);
        }

        public IEnumerable<Timezone> GetAll()
        {
            return _context.Timezone.ToList<Timezone>();
        }

        public Timezone GetByUserId(string id)
        {
            return _context.Timezone.FirstOrDefault(t => t.Expert.UserId == id);
        }
    }
}
