using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using stutor_core.Database;
using stutor_core.Models;
using stutor_core.Models.Sql;
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

        public IEnumerable<Topic> GetExpertTopicsByUserId(string userId)
        {
            return _repo.GetExpertTopicsByUserId(userId);
        }

        public IEnumerable<Order> GetExpertOrdersByUserId(string userId)
        {
            return _repo.GetExpertOrdersByUserId(userId);
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
    }
}
