using stutor_core.Models.Sql;
using stutor_core.Models.ViewModels;
using System.Collections.Generic;

namespace stutor_core.Repositories.Interfaces
{
    public interface IExpertRepository
    {
        Expert Get(string id);

        decimal GetExpertPrice(string expertId, int topicId);

        IEnumerable<Topic> GetExpertTopicsByUserId(string userId);

        TopicExpertsReturnVM GetTopicExpertsByTopicId(SelectedTopicVM selectedTopicVm);

        int Register(ExpertApplication application);

        bool IsActive(string userId);

        bool ToggleIsActive(string userId, bool isActive);

        bool UpdateTimezone(string userId, int timezoneId);

        string GetPhoneById(string expertId);

        bool RevokeTopicExpert(int topicExpertId, string expertId);

        IEnumerable<Specialty> GetSpecialties(int topicExpertId);

        bool UpdateTopicExpertSpecialties(int topicExpertId, int[] specialtyIds);

        bool HasIncompleteOrders(string userId);
    }
}
