using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using stutor_core.Database;
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

        public IEnumerable<TopicExpert> GetTopicExpertsByTopicId(int topicId)
        {
            return _repo.GetTopicExpertsByTopicId(topicId);
        }
    }
}
