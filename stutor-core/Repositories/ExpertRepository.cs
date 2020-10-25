using Microsoft.EntityFrameworkCore;
using stutor_core.Database;
using stutor_core.Models.Sql;
using stutor_core.Models.ViewModels;
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

        public IEnumerable<Topic> GetExpertTopicsByUserEmail(string userEmail)
        {
            return _context.Topic.Where(t => t.Id == t.TopicExpert.TopicId && t.TopicExpert.Expert.UserEmail == userEmail).Include(x => x.TopicExpert);
        }

        public IEnumerable<Order> GetExpertOrdersByUserEmail(string userEmail)
        {
            return _context.Order.Where(o => o.ExpertId == o.Expert.Id && o.Expert.UserEmail == userEmail).Include(x => x.Topic);
        }

        public TopicExpertsReturnVM GetTopicExpertsByTopicId(SelectedTopicVM selectedTopicVm)
        {
            Random rnd = new Random();

            TopicExpertsReturnVM result = new TopicExpertsReturnVM();
            result.LocalExperts = _context.TopicExpert.Where(e => e.TopicId == selectedTopicVm.TopicId // Retrieve experts on topicId
                                                && e.Expert.Timezone.TZName == selectedTopicVm.UserTimezone && e.Expert.IsActive
                                                && AvailabilityParser.IsAvailable(e.Availability, e.Expert.Timezone.TZName)// where timezones match and where available
                                                ).GroupBy(g => g.Rating) // group by rating
                                                .Select(x => x.ElementAt(rnd.Next(0, x.Count()))); // randomly select 1 expert with each rating

            if (result.LocalExperts.Count() < 3) // if there are few results try opening up the criteria to more timezones.
            {
                result.DistantExperts = _context.TopicExpert.Where(e => e.TopicId == selectedTopicVm.TopicId // Retrieve experts on topicId
                                                && e.Expert.Timezone.TZName != selectedTopicVm.UserTimezone && e.Expert.IsActive
                                                && AvailabilityParser.IsAvailable(e.Availability, e.Expert.Timezone.TZName) // where currently available and not in the same timezone
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

        public bool IsActive(string userEmail)
        {
            return _context.Expert.Single(x => x.UserEmail == userEmail).IsActive;
        }

        public bool ToggleIsActive(string userEmail, bool isActive)
        {
            var record = _context.Expert.Single(e => e.UserEmail == userEmail && e.IsActive == isActive);
            record.IsActive = !isActive;
            return (_context.SaveChanges() == 1) ? !isActive : isActive;
        }

        public bool UpdateTimezone(string userEmail, int timezoneId)
        {
            var record = _context.Expert.FirstOrDefault(x => x.UserEmail == userEmail);
            record.TimezoneId = timezoneId;
            return _context.SaveChanges() == 1;
        }

        public string GetPhoneById(string expertId)
        {
            return _context.User.FirstOrDefault(x => x.Expert.Id == expertId).Phone;
        }
    }
}
