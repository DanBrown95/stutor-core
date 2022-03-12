using stutor_core.Database;
using System.Linq;
using stutor_core.Repositories.Interfaces;
using stutor_core.Models.ViewModels;
using Microsoft.Extensions.Configuration;

namespace stutor_core.Repositories
{
    public class LocationRepository : BaseRepository, ILocationRepository
    {
        public LocationRepository(ApplicationDbContext context, IConfiguration config) : base(context, config) { }

        public LocationData GetLocationByUserId(string id)
        {
            var ex =  _context.Expert.FirstOrDefault(t => t.UserId == id);
            return new LocationData() { Address = ex.Address, Coords = new Coordinates() { Lat = ex.Latitude, Lng = ex.Longitude } };
        }
    }
}
