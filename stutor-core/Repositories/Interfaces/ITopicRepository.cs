using stutor_core.Models;
using stutor_core.Models.Sql;
using System.Collections.Generic;

namespace stutor_core.Repositories.Interfaces
{
    public interface ITopicRepository
    {
        void Add(Topic top);

        Topic Get(int ID);

        IEnumerable<Topic> GetAll();

        IEnumerable<TopicExpert> GetTopicExperts(int topicId);

        IEnumerable<Topic> GetTopicsByCategory(int categoryId);

        IEnumerable<Topic> GetTopicsBySubstring(string sub);

        int SubmitTopicRequest(TopicRequest request);

        IEnumerable<Specialty> GetAllSpecialtiesByTopicId(int id);

        IEnumerable<Topic> GetRelatedTopics(int id);
    }
}
