using stutor_core.Database;
using stutor_core.Models.Sql;
using stutor_core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Services
{
    public class CareerService
    {
        private readonly CareerRepository _repo;

        public CareerService(ApplicationDbContext context)
        {
            _repo = new CareerRepository(context);
        }

        public IEnumerable<AvailableJob> GetAllAvailableJobs()
        {
            return _repo.GetAllAvailableJobs();
        }
    }
}
