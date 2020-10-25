using System.Collections.Generic;
using stutor_core.Database;
using stutor_core.Models.Sql;
using stutor_core.Models.ViewModels;
using stutor_core.Repositories;

namespace stutor_core.Services
{
    public class ExpertService
    {
        private readonly ExpertRepository _repo;

        public ExpertService(ApplicationDbContext context)
        {
            _repo = new ExpertRepository(context);
        }

        public Expert Get(string id)
        {
            return _repo.Get(id);
        }

        public decimal GetExpertPrice(string expertId, int topicId)
        {
            return _repo.GetExpertPrice(expertId, topicId);
        }

        public IEnumerable<Topic> GetExpertTopicsByUserEmail(string userEmail)
        {
            return _repo.GetExpertTopicsByUserEmail(userEmail);
        }

        public IEnumerable<Order> GetExpertOrdersByUserEmail(string userEmail)
        {
            return _repo.GetExpertOrdersByUserEmail(userEmail);
        }

        public TopicExpertsReturnVM GetTopicExpertsByTopicId(SelectedTopicVM selectedTopicVm)
        {
            return _repo.GetTopicExpertsByTopicId(selectedTopicVm);
        }

        public bool IsActive(string userEmail)
        {
            return _repo.IsActive(userEmail);
        }

        public bool ToggleIsActive(string userEmail, bool isActive)
        {
            return _repo.ToggleIsActive(userEmail, isActive);
        }

        public int Register(ExpertApplication application)
        {
            return _repo.Register(application);
        }

        public bool UpdateTimezone(string userEmail, int timezoneId)
        {
            return _repo.UpdateTimezone(userEmail, timezoneId);
        }

        public string GetPhoneById(string expertId)
        {
            return _repo.GetPhoneById(expertId);
        }
    }
}
