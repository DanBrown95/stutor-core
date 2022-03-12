using Microsoft.EntityFrameworkCore;
using stutor_core.Database;
using stutor_core.Models.Sql;
using stutor_core.Models.ViewModels;
using stutor_core.Services;
using stutor_core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using stutor_core.Repositories.Interfaces;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using stutor_core.Models.Sql.customTypes;

namespace stutor_core.Repositories
{
    public class ExpertRepository : BaseRepository, IExpertRepository
    {
        public ExpertRepository(ApplicationDbContext context, IConfiguration config) : base(context,  config) { }

        public Expert Get(string id)
        {
           return _context.Expert.Where(e => e.Id == id).Include(x => x.User).FirstOrDefault();
        }

        public decimal GetExpertPrice(string expertId, int topicId)
        {
            return _context.Expert.Where(e => e.Id == expertId).Include(x => x.TopicExpert).FirstOrDefault().TopicExpert.FirstOrDefault(t => t.TopicId == topicId).Price;
        }

        public IEnumerable<Topic> GetExpertTopicsByUserId(string userId)
        {
            var e  = _context.Topic.Where(t => t.Id == t.TopicExpert.TopicId && t.TopicExpert.Expert.UserId == userId && t.TopicExpert.IsActive == true).Include(x => x.TopicExpert).ToList();
            return e;
        }

        //public IEnumerable<Order> GetExpertOrdersByUserId(string userId)
        //{
        //    return _context.Order.Where(o => o.ExpertId == o.Expert.Id && o.Expert.UserId == userId).Include(x => x.Topic);
        //}

        public TopicExpertsReturnVM GetTopicExpertsByTopicId(SelectedTopicVM selectedTopicVm)
        {
            Random rnd = new Random();
            var stripeService = new StripeService();

            TopicExpertsReturnVM result = new TopicExpertsReturnVM();
            var param = new DynamicParameters();
            param.Add("@TopicId", selectedTopicVm.TopicId, dbType: DbType.Int32);
            param.Add("@UserId", selectedTopicVm.RequestingUserId, dbType: DbType.String);
            param.Add("@Lat", selectedTopicVm.UserCoordinates.Lat, dbType: DbType.Decimal);
            param.Add("@Lng", selectedTopicVm.UserCoordinates.Lng, dbType: DbType.Decimal);
            param.Add("@Radius", 100, dbType: DbType.Int32);

            using (var db = DBConnection())
            {
                result.LocalExperts = db.Query<TopicExpertsByTopic>("spExpertsByGeographicalCoordinatesGet", param, commandType: CommandType.StoredProcedure)
                                      .Where(e => AvailabilityParser.IsAvailable(e.Availability, new Coordinates() { Lat = e.Latitude, Lng = e.Longitude })
                                                                 && stripeService.CustomerHasSources(e.CustomerId))
                                      .GroupBy(g => g.Rating)
                                      .Select(x => x.ElementAt(rnd.Next(0, x.Count())))
                                      .ToList();
            }

            if (result.LocalExperts.Count() < 3) // if there are few results try opening up the criteria to a larger radius.
            {
                using (var db = DBConnection())
                {
                    param.Add("@Radius", 500, dbType: DbType.Int32); // increase radius to 500miles

                    result.DistantExperts = db.Query<TopicExpertsByTopic>("spExpertsByGeographicalCoordinatesGet", param, commandType: CommandType.StoredProcedure)
                                          .Where(e => AvailabilityParser.IsAvailable(e.Availability, new Coordinates() { Lat = e.Latitude, Lng = e.Longitude }) 
                                                                     && stripeService.CustomerHasSources(e.CustomerId))
                                          .GroupBy(g => g.Rating)
                                          .Select(x => x.ElementAt(rnd.Next(0, x.Count())))
                                          .ToList();
                }
            }

            // Retrieve expert specialties
            foreach (var ex in result.DistantExperts)
            {
                ex.TopicExpertSpecialty = _context.TopicExpertSpecialty.Where(x => x.TopicExpertId == ex.Id).ToList();
                foreach (var sp in ex.TopicExpertSpecialty)
                {
                    sp.Specialty = _context.Specialty.FirstOrDefault(x => x.Id == sp.SpecialtyId);
                }
            }

            foreach (var ex in result.LocalExperts)
            {
                ex.TopicExpertSpecialty = _context.TopicExpertSpecialty.Where(x => x.TopicExpertId == ex.Id).ToList();
                foreach (var sp in ex.TopicExpertSpecialty)
                {
                    sp.Specialty = _context.Specialty.FirstOrDefault(x => x.Id == sp.SpecialtyId);
                }
            }

            return result;
        }

        public int Register(ExpertApplication application)
        {
            _context.ExpertApplication.Add(application);
            _context.SaveChanges();
            int id = application.Id;
            return id;
        }

        public bool IsActive(string userId)
        {
            return _context.Expert.Single(x => x.UserId == userId).IsActive;
        }

        public bool ToggleIsActive(string userId, bool isActive)
        {
            var record = _context.Expert.Single(e => e.UserId == userId && e.IsActive == isActive);
            record.IsActive = !isActive;
            return (_context.SaveChanges() == 1) ? !isActive : isActive;
        }

        public bool UpdateLocation(string userId, LocationData location)
        {
            var record = _context.Expert.FirstOrDefault(x => x.UserId == userId);
            record.Address = location.Address;
            record.Latitude = location.Coords.Lat;
            record.Longitude = location.Coords.Lng;
            return _context.SaveChanges() == 1;
        }

        public string GetPhoneById(string expertId)
        {
            return _context.User.FirstOrDefault(x => x.Expert.Id == expertId).Phone;
        }

        public bool RevokeTopicExpert(int topicExpertId, string expertId)
        {
            var existingTopicExpert = _context.TopicExpert.FirstOrDefault(x => x.Id == topicExpertId && x.ExpertId == expertId);
            if(existingTopicExpert.Id > 0)
            {
                existingTopicExpert.IsActive = false;
                return _context.SaveChanges() == 1;
            }
            return false;
        }

        public IEnumerable<Specialty> GetSpecialties(int topicExpertId)
        {
            return _context.TopicExpertSpecialty.Where(x => x.TopicExpertId == topicExpertId).Include(x => x.Specialty).Select(y => y.Specialty);
        }

        public bool UpdateTopicExpertSpecialties(int topicExpertId, int[] specialtyIds)
        {
            var existingSpecialties = _context.TopicExpertSpecialty.Where(x => x.TopicExpertId == topicExpertId);
            if(existingSpecialties.Count() > 0)
            {
                _context.TopicExpertSpecialty.RemoveRange(existingSpecialties);
                _context.SaveChanges();
            }
            
            if(specialtyIds.Length > 0)
            {
                var newSpecialties = new List<TopicExpertSpecialty>();
                foreach (var id in specialtyIds)
                {
                    var specialty = new TopicExpertSpecialty() { SpecialtyId = id, TopicExpertId = topicExpertId };
                    newSpecialties.Add(specialty);
                }
                _context.TopicExpertSpecialty.AddRange(newSpecialties);
                _context.SaveChanges();
            }
            return true;
        }

        public bool HasIncompleteOrders(string userId)
        {
            return _context.Order.Where(x => x.Expert.UserId == userId && x.Status == Models.Enumerations.OrderStatus.Unanswered).Count() > 0;
        }
    }
}
