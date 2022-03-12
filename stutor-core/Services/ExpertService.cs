using System.Collections.Generic;
using stutor_core.Models.Sql;
using stutor_core.Models.ViewModels;
using stutor_core.Repositories.Interfaces;
using stutor_core.Services.Interfaces;

namespace stutor_core.Services
{
    public class ExpertService : IExpertService
    {
        private readonly IExpertRepository _repo;

        public ExpertService(IExpertRepository repo)
        {
            _repo = repo;
        }

        public Expert Get(string id)
        {
            return _repo.Get(id);
        }

        public decimal GetExpertPrice(string expertId, int topicId)
        {
            return _repo.GetExpertPrice(expertId, topicId);
        }

        public IEnumerable<Topic> GetExpertTopicsByUserId(string userId)
        {
            return _repo.GetExpertTopicsByUserId(userId);
        }

        public TopicExpertsReturnVM GetTopicExpertsByTopicId(SelectedTopicVM selectedTopicVm)
        {
            return _repo.GetTopicExpertsByTopicId(selectedTopicVm);
        }

        public bool IsActive(string userId)
        {
            return _repo.IsActive(userId);
        }

        public bool ToggleIsActive(string userId, bool isActive)
        {
            return _repo.ToggleIsActive(userId, isActive);
        }

        public int Register(ExpertApplication application)
        {
            return _repo.Register(application);
        }

        public bool UpdateLocation(string userId, LocationData location)
        {
            return _repo.UpdateLocation(userId, location);
        }

        public string GetPhoneById(string expertId)
        {
            return _repo.GetPhoneById(expertId);
        }

        public bool RevokeTopicExpert(int topicExpertId, string expertId)
        {
            return _repo.RevokeTopicExpert(topicExpertId, expertId);
        }

        public IEnumerable<Specialty> GetSpecialties(int topicExpertId)
        {
            return _repo.GetSpecialties(topicExpertId);
        }

        public bool UpdateTopicExpertSpecialties(int topicExpertId, int[] specialtyIds)
        {
            return _repo.UpdateTopicExpertSpecialties(topicExpertId, specialtyIds);
        }

        public bool HasIncompleteOrders(string userId)
        {
            return _repo.HasIncompleteOrders(userId);
        }
    }
}
