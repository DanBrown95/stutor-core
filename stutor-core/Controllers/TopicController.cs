using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using stutor_core.Models;
using System.Collections.Generic;
using stutor_core.Services;
using stutor_core.Database;
using stutor_core.Models.Sql;

namespace stutor_core.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TopicController : Controller
    {
        private readonly TopicService _topicService;

        public TopicController(ApplicationDbContext db)
        {
            _topicService = new TopicService(db);
        }

        [HttpPost]
        [Authorize]
        public int SubmitRequest(TopicRequest requestedTopic)
        {
            return _topicService.SubmitTopicRequest(requestedTopic);
        }
        
        [HttpPost]
        public IEnumerable<Topic> GetTopicsByCategory([FromBody] int categoryId)
        {
            return _topicService.GetTopicsByCategory(categoryId);
        }

        [HttpPost]
        public Topic Get([FromBody] int topicId)
        {
            return _topicService.Get(topicId);
        }

        [HttpPost]
        public IEnumerable<Topic> GetBySubstring([FromBody] string substring)
        {
            return _topicService.GetTopicsBySubstring(substring);
        }
    }
}