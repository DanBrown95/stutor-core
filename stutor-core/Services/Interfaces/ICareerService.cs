using System.Collections.Generic;
using stutor_core.Models.Sql;

namespace stutor_core.Services.Interfaces
{
    public interface ICareerService
    {
        IEnumerable<AvailableJob> GetAllAvailableJobs();
    }
}
