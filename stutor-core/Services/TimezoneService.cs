using stutor_core.Database;
using stutor_core.Models.Sql;
using stutor_core.Repositories;
using System.Collections.Generic;

namespace stutor_core.Services
{
    public class TimezoneService
    {
        private readonly TimezoneRepository _repo;

        public TimezoneService(ApplicationDbContext context)
        {
            _repo = new TimezoneRepository(context);
        }

        public Timezone Get(int id)
        {
            return _repo.Get(id);
        }

        public IEnumerable<Timezone> GetAll()
        {
            return _repo.GetAll();
        }

        public void Add(Timezone timezone)
        {
            _repo.Add(timezone);
        }

        public Timezone GetByUserId(string id)
        {
            return _repo.GetByUserId(id);
        }
    }
}
