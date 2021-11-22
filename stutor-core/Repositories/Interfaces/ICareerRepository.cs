using stutor_core.Models.Sql;
using System.Collections.Generic;

namespace stutor_core.Repositories.Interfaces
{
    public interface ICareerRepository
    {
        IEnumerable<AvailableJob> GetAllAvailableJobs();
    }
}
