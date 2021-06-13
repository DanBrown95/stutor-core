using stutor_core.Database;
using stutor_core.Models.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Repositories
{
    public class CareerRepository
    {
        private ApplicationDbContext _context { get; set; }

        public CareerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<AvailableJob> GetAllAvailableJobs()
        {
            return _context.AvailableJob.ToList();
        }
    }
}
