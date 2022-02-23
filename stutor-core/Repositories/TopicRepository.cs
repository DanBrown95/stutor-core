using stutor_core.Database;
using stutor_core.Models;
using stutor_core.Models.Sql;
using System.Collections.Generic;
using System.Linq;
using stutor_core.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace stutor_core.Repositories
{
    public class TopicRepository : BaseRepository, ITopicRepository
    {
        public TopicRepository(ApplicationDbContext context, IConfiguration config) : base(context, config) { }

        public void Add(Topic top)
        {
            _context.Topic.Add(top);
            _context.SaveChanges();
        }

        public Topic Get(int ID)
        {
            return _context.Topic.FirstOrDefault(e => e.Id == ID);
        }

        public IEnumerable<Topic> GetAll()
        {
            return _context.Topic.ToList<Topic>();
        }

        public IEnumerable<TopicExpert> GetTopicExperts(int topicId)
        {
            return _context.TopicExpert.Where(t => t.TopicId == topicId);
        }

        public IEnumerable<Topic> GetTopicsByCategory(int categoryId)
        {
            return _context.Topic.ToList<Topic>().Where(t => t.CategoryId == categoryId);
        }

        public IEnumerable<Topic> GetTopicsBySubstring(string sub)
        {
            return _context.Topic.Where(x => x.Name.Contains(sub));
        }

        public int SubmitTopicRequest(TopicRequest request)
        {
            _context.TopicRequest.Add(request);
            return _context.SaveChanges();
        }

        public IEnumerable<Specialty> GetAllSpecialtiesByTopicId(int id)
        {
            return _context.Specialty.Where(x => x.TopicId == id);
        }

        public IEnumerable<Topic> GetRelatedTopics(int id)
        {
            int categoryId = _context.Topic.FirstOrDefault(x => x.Id == id).CategoryId;
            if (categoryId == 0)
            {
                return new List<Topic>();
            }
            return _context.Topic.Where(x => x.CategoryId == categoryId && x.Id != id).Take(6);
        }
    }
}
