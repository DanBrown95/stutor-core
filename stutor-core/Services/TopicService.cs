using stutor_core.Database;
using stutor_core.Models;
using stutor_core.Models.Sql;
using stutor_core.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace stutor_core.Services
{
    public class TopicService
    {
        private readonly TopicRepository _repo;

        public TopicService(ApplicationDbContext context)
        {
            _repo = new TopicRepository(context);
        }

        public Topic Get(int id)
        {
            return _repo.Get(id);
        }

        public IEnumerable<Topic> GetAll()
        {
            return _repo.GetAll();
        }

        public void Add(Topic topic)
        {
            _repo.Add(topic);
        }

        public IEnumerable<TopicExpert> GetTopicExperts(int topicId)
        {
            return _repo.GetTopicExperts(topicId);
        }

        public IEnumerable<Topic> GetTopicsByCategory(int categoryId)
        {
            return _repo.GetTopicsByCategory(categoryId);
        }

        public IEnumerable<Topic> GetTopicsBySubstring(string substring)
        {
            return _repo.GetTopicsBySubstring(substring);
        }

        public int SubmitTopicRequest(TopicRequest request)
        {
            return _repo.SubmitTopicRequest(request);
        }
    }
}
