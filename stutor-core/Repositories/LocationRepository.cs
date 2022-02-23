using stutor_core.Database;
using stutor_core.Models.Sql;
using System.Collections.Generic;
using System.Linq;
using stutor_core.Repositories.Interfaces;
using stutor_core.Models.ViewModels;
using Microsoft.Extensions.Configuration;

namespace stutor_core.Repositories
{
    public class LocationRepository : BaseRepository, ILocationRepository
    {
        public LocationRepository(ApplicationDbContext context, IConfiguration config) : base(context, config) { }

        public void Add(Timezone timezone)
        {
            _context.Add(timezone);
            _context.SaveChanges();
        }

        public Timezone Get(int ID)
        {
            return _context.Timezone.FirstOrDefault(e => e.Id == ID);
        }

        public IEnumerable<Timezone> GetAll()
        {
            return _context.Timezone.ToList<Timezone>();
        }

        public Timezone GetTimezoneByUserId(string id)
        {
            return _context.Timezone.FirstOrDefault(t => t.Expert.UserId == id);
        }

        public LocationData GetLocationByUserId(string id)
        {
            var ex =  _context.Expert.FirstOrDefault(t => t.UserId == id);
            return new LocationData() { Address = ex.Address, Coords = new Coordinates() { Lat = ex.Latitude, Lng = ex.Longitude } };
        }
    }
}
