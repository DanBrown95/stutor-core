using stutor_core.Models.Sql;
using System.Collections.Generic;
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

        public Timezone Get(int id)
        {
            return _repo.Get(id);
        }

        public LocationData GetLocationByUserId (string id)
        {
            return _repo.GetLocationByUserId(id);
        }

        public IEnumerable<Timezone> GetAll()
        {
            return _repo.GetAll();
        }

        public void Add(Timezone timezone)
        {
            _repo.Add(timezone);
        }

        public Timezone GetTimezoneByUserId(string id)
        {
            return _repo.GetTimezoneByUserId(id);
        }
    }
}
