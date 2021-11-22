using stutor_core.Models.Sql;
using System.Collections.Generic;
using stutor_core.Services.Interfaces;
using stutor_core.Repositories.Interfaces;

namespace stutor_core.Services
{
    public class TimezoneService : ITimezoneService
    {
        private readonly ITimezoneRepository _repo;

        public TimezoneService(ITimezoneRepository repo)
        {
            _repo = repo;
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
