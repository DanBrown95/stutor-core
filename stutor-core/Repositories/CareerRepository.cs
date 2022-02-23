using Microsoft.Extensions.Configuration;
using stutor_core.Database;
using stutor_core.Models.Sql;
using stutor_core.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace stutor_core.Repositories
{
    public class CareerRepository : BaseRepository, ICareerRepository
    {

        public CareerRepository(ApplicationDbContext context, IConfiguration config) : base(context, config) { }

        public IEnumerable<AvailableJob> GetAllAvailableJobs()
        {
            return _context.AvailableJob.ToList();
        }
    }
}
