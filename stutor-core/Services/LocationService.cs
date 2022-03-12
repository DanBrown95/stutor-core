using stutor_core.Services.Interfaces;
using stutor_core.Repositories.Interfaces;
using stutor_core.Models.ViewModels;

namespace stutor_core.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _repo;

        public LocationService(ILocationRepository repo)
        {
            _repo = repo;
        }

        public LocationData GetLocationByUserId (string id)
        {
            return _repo.GetLocationByUserId(id);
        }
    }
}
