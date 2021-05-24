using Microsoft.EntityFrameworkCore;
using stutor_core.Database;
using stutor_core.Models.Sql;
using stutor_core.Models.ViewModels;
using stutor_core.Services;
using stutor_core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace stutor_core.Repositories
{
    public class ExpertRepository
    {
        private ApplicationDbContext _context { get; set; }

        public ExpertRepository(ApplicationDbContext context)
        {
            _context = context;
        }

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
            return _context.Topic.Where(t => t.Id == t.TopicExpert.TopicId && t.TopicExpert.Expert.UserId == userId).Include(x => x.TopicExpert);
        }

        public IEnumerable<Order> GetExpertOrdersByUserId(string userId)
        {
            return _context.Order.Where(o => o.ExpertId == o.Expert.Id && o.Expert.UserId == userId).Include(x => x.Topic);
        }

        public TopicExpertsReturnVM GetTopicExpertsByTopicId(SelectedTopicVM selectedTopicVm)
        {
            Random rnd = new Random();
            var sc = new StripeController();

            TopicExpertsReturnVM result = new TopicExpertsReturnVM();
            result.LocalExperts = _context.TopicExpert.Where(e => e.TopicId == selectedTopicVm.TopicId // Retrieve experts on topicId
                                                && e.Expert.Timezone.TZName == selectedTopicVm.UserTimezone 
                                                && e.Expert.IsActive
                                                && e.Expert.UserId != selectedTopicVm.RequestingUserId
                                                && AvailabilityParser.IsAvailable(e.Availability, e.Expert.Timezone.TZName)
                                                && sc.CustomerHasSources(e.Expert.User.CustomerId) == true
                                                // where timezones match, where available and experts who have payment methods stored on their stripe account
                                                ).GroupBy(g => g.Rating) // group by rating
                                                .Select(x => x.ElementAt(rnd.Next(0, x.Count()))); // randomly select 1 expert with each rating

            if (result.LocalExperts.Count() < 3) // if there are few results try opening up the criteria to more timezones.
            {
                result.DistantExperts = _context.TopicExpert.Where(e => e.TopicId == selectedTopicVm.TopicId // Retrieve experts on topicId
                                                && e.Expert.Timezone.TZName != selectedTopicVm.UserTimezone 
                                                && e.Expert.IsActive
                                                && e.Expert.UserId != selectedTopicVm.RequestingUserId
                                                && AvailabilityParser.IsAvailable(e.Availability, e.Expert.Timezone.TZName) // where currently available and not in the same timezone
                                                && sc.CustomerHasSources(e.Expert.User.CustomerId) == true
                                                ).GroupBy(g => g.Rating) // group by rating
                                                .Select(x => x.ElementAt(rnd.Next(0, x.Count()))); // randomly select 1 expert with each rating
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
    }
}
