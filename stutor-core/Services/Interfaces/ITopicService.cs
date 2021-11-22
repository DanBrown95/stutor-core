using stutor_core.Models;
using stutor_core.Models.Sql;
using System.Collections.Generic;

namespace stutor_core.Services.Interfaces
{
    public interface ITopicService
    {
        Topic Get(int id);

        IEnumerable<Topic> GetAll();

        void Add(Topic topic);

        IEnumerable<TopicExpert> GetTopicExperts(int topicId);

        IEnumerable<Topic> GetTopicsByCategory(int categoryId);

        IEnumerable<Topic> GetTopicsBySubstring(string substring);

        int SubmitTopicRequest(TopicRequest request);

        IEnumerable<Specialty> GetAllSpecialtiesByTopicId(int id);

        IEnumerable<Topic> GetRelatedTopics(int id);
    }
}
