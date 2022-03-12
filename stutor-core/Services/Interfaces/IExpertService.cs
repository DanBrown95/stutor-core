using stutor_core.Models.Sql;
using stutor_core.Models.ViewModels;
using System.Collections.Generic;

namespace stutor_core.Services.Interfaces
{
    public interface IExpertService
    {
        Expert Get(string id);

        decimal GetExpertPrice(string expertId, int topicId);

        IEnumerable<Topic> GetExpertTopicsByUserId(string userId);

        TopicExpertsReturnVM GetTopicExpertsByTopicId(SelectedTopicVM selectedTopicVm);

        bool IsActive(string userId);

        bool ToggleIsActive(string userId, bool isActive);

        int Register(ExpertApplication application);
        
        bool UpdateLocation(string userId, LocationData location);

        string GetPhoneById(string expertId);

        bool RevokeTopicExpert(int topicExpertId, string expertId);

        IEnumerable<Specialty> GetSpecialties(int topicExpertId);

        bool UpdateTopicExpertSpecialties(int topicExpertId, int[] specialtyIds);

        bool HasIncompleteOrders(string userId);
    }
}
