using stutor_core.Models;
using stutor_core.Models.Sql;
using System.Collections.Generic;
using stutor_core.Services.Interfaces;
using stutor_core.Repositories.Interfaces;

namespace stutor_core.Services
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _repo;

        public TopicService(ITopicRepository repo)
        {
            _repo = repo;
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

        public IEnumerable<Specialty> GetAllSpecialtiesByTopicId(int id)
        {
            return _repo.GetAllSpecialtiesByTopicId(id);
        }

        public IEnumerable<Topic> GetRelatedTopics(int id)
        {
            return _repo.GetRelatedTopics(id);
        }
    }
}
