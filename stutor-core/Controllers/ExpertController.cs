using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using stutor_core.Database;
using stutor_core.Models;
using stutor_core.Models.Sql;
using stutor_core.Services;

namespace stutor_core.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExpertController : Controller
    {
        private IHostingEnvironment _environment;
        private ExpertService _expertService;
        private ApplicationDbContext _db;

        public ExpertController(IHostingEnvironment environment, ApplicationDbContext db)
        {
            _db = db;
            _expertService = new ExpertService(_db);
            _environment = environment;
        }

        [HttpPost]
        public async Task<JsonResult> UploadDocuments(IFormCollection formData)
        {
            string wwwPath = _environment.WebRootPath;
            string contentPath = _environment.ContentRootPath;

            string path = Path.Combine(_environment.ContentRootPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<string> uploadedFiles = new List<string>();
            foreach (var file in formData.Files)
            {
                if (file.Length > 0)
                {
                    using (var filestream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(filestream);
                    }
                }
            }
            return Json(new { status = 9000 });
        }

        [HttpPost]
        public async Task<JsonResult> Register(ExpertRegistration formData)
        {
            return Json(new { status = 9000 });
        }

        [HttpPost]
        public Timezone ExpertTimezone([FromBody] string userId)
        {
            return new TimezoneService(_db).GetByUserId(userId);
        }

        [HttpPost]
        public IEnumerable<Topic> TopicsByUserId([FromBody] string userId)
        {
            return _expertService.GetExpertTopicsByUserId(userId);
        }

        [HttpPost]
        public IEnumerable<Order> OrdersByUserId([FromBody] string userId)
        {
            OrderService orderService = new OrderService(_db);
            return orderService.GetExpertOrdersByUserId(userId);
        }

        [HttpPost]
        public IEnumerable<TopicExpert> TopicExpertsByTopicId([FromBody] SelectedTopicVM vm)
        {
            return _expertService.GetTopicExpertsByTopicId(vm.TopicId);
        } 
    }
}