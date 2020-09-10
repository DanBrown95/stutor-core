using Microsoft.EntityFrameworkCore;
using stutor_core.Database;
using stutor_core.Models.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public IEnumerable<Topic> GetExpertTopicsByUserId(string userId)
        {
            return _context.Topic.Where(t => t.Id == t.TopicExpert.TopicId && t.TopicExpert.Expert.UserId == userId).Include(x => x.TopicExpert);
        }

        public IEnumerable<Order> GetExpertOrdersByUserId(string userId)
        {
            return _context.Order.Where(o => o.ExpertId == o.Expert.Id && o.Expert.UserId == userId).Include(x => x.Topic);
        }

        public IEnumerable<TopicExpert> GetTopicExpertsByTopicId(int topicId)
        {
            return _context.TopicExpert.Where(e => e.TopicId == topicId);
        }


    }
}
