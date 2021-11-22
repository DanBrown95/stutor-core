using stutor_core.Models.Sql;
using stutor_core.Repositories.Interfaces;
using stutor_core.Services.Interfaces;
using System.Collections.Generic;

namespace stutor_core.Services
{
    public class CareerService : ICareerService
    {
        private readonly ICareerRepository _careerRepository;

        public CareerService(ICareerRepository repo)
        {
            _careerRepository = repo;
        }

        public IEnumerable<AvailableJob> GetAllAvailableJobs()
        {
            return _careerRepository.GetAllAvailableJobs();
        }
    }
}
