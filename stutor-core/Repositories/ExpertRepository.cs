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

namespace stutor_core.Repositories
{
    public class ExpertRepository : BaseRepository, IExpertRepository
    {
        public ExpertRepository(ApplicationDbContext context) : base(context) { }

        public Expert Get(string id)
        {
           return _context.Expert.FirstOrDefault(e => e.Id == id);
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
            var sc = new StripeService();

            TopicExpertsReturnVM result = new TopicExpertsReturnVM();
            result.LocalExperts = _context.TopicExpert.Where(e => e.TopicId == selectedTopicVm.TopicId // Retrieve experts on topicId
                                                && e.IsActive // Ensure they haven't revoked their status for that topic
                                                && e.Expert.Timezone.TZName == selectedTopicVm.UserTimezone 
                                                && e.Expert.IsActive
                                                && e.Expert.UserId != selectedTopicVm.RequestingUserId
                                                && AvailabilityParser.IsAvailable(e.Availability, e.Expert.Timezone.TZName)
                                                && sc.CustomerHasSources(e.Expert.User.CustomerId) == true
                                                // where timezones match, where available and experts who have payment methods stored on their stripe account
                                                ).Include(x => x.TopicExpertSpecialty).GroupBy(g => g.Rating) // group by rating
                                                .Select(x => x.ElementAt(rnd.Next(0, x.Count()))).ToList(); // randomly select 1 expert with each rating

            if (result.LocalExperts.Count() < 3) // if there are few results try opening up the criteria to more timezones.
            {
                result.DistantExperts = _context.TopicExpert.Where(e => e.TopicId == selectedTopicVm.TopicId // Retrieve experts on topicId
                                                && e.IsActive // Ensure they haven't revoked their status for that topic
                                                && e.Expert.Timezone.TZName != selectedTopicVm.UserTimezone 
                                                && e.Expert.IsActive
                                                && e.Expert.UserId != selectedTopicVm.RequestingUserId
                                                && AvailabilityParser.IsAvailable(e.Availability, e.Expert.Timezone.TZName) // where currently available and not in the same timezone
                                                && sc.CustomerHasSources(e.Expert.User.CustomerId) == true
                                                ).Include(x => x.TopicExpertSpecialty).GroupBy(g => g.Rating) // group by rating
                                                .Select(x => x.ElementAt(rnd.Next(0, x.Count()))).ToList(); // randomly select 1 expert with each rating
            }

            // Retrieve expert specialties
            foreach (var ex in result.DistantExperts)
            {
                foreach (var sp in ex.TopicExpertSpecialty)
                {
                    sp.Specialty = _context.Specialty.FirstOrDefault(x => x.Id == sp.SpecialtyId);
                }
            }

            foreach (var ex in result.LocalExperts)
            {
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

        public bool UpdateTimezone(string userId, int timezoneId)
        {
            var record = _context.Expert.FirstOrDefault(x => x.UserId == userId);
            record.TimezoneId = timezoneId;
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
