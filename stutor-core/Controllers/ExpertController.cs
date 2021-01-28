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
using stutor_core.Models.ViewModels;
using stutor_core.Repositories;
using stutor_core.Services;

namespace stutor_core.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExpertController : Controller
    {
        private ExpertService _expertService;
        private ApplicationDbContext _db;

        public ExpertController(ApplicationDbContext db)
        {
            _db = db;
            _expertService = new ExpertService(_db);
        }

        [HttpPost]
        public async Task<JsonResult> UploadDocuments(IFormCollection documents)
        {
            if(documents.Files.Count < 1)
            {
                return Json(new { status = 400 });
            }

            var googleCloudRepo = new GoogleCloudRepository();
            await googleCloudRepo.UploadToBucketAsync(documents);

            return Json(new { status = 200 });
        }

        [HttpPost]
        public int Register(ExpertRegistrationVM formData)
        {
            var application = new ExpertApplication{
                UserId = formData.UserId,
                TopicId = formData.TopicId,
                TimezoneId = formData.TimezoneId,
                WebsiteUrl = formData.WebsiteUrl,
                LinkedinUrl = formData.LinkedinUrl,
                Certifications = formData.Certifications,
                YearsOfExperience = formData.YearsOfExperience,
                Notes = formData.Notes
            };

            var availability = "{\"days\":[\"" + string.Join("\",\"", formData.SelectedDays) + "\"]";
            if (formData.WeekdayHours != "null-null")
            {
                availability += ",\"weekdayHours\":\"" + formData.WeekdayHours + "\"";
            }
            if(formData.WeekendHours != "null-null")
            {
                availability += ",\"weekendHours\":\"" + formData.WeekendHours + "\"";
            }
            availability += "}";

            application.Availability = availability;//availability.Remove(availability.Length -1, 1);
            return _expertService.Register(application);
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
        public TopicExpertsReturnVM TopicExpertsByTopicId([FromBody] SelectedTopicVM vm)
        {
            return _expertService.GetTopicExpertsByTopicId(vm);
        }
        
        [HttpPost]
        public bool IsActive([FromBody] string userId)
        {
            return _expertService.IsActive(userId);
        }

        [HttpPost]
        public bool ToggleIsActive([FromBody] ToggleActive vm)
        {
            return _expertService.ToggleIsActive(vm.UserId, vm.IsActive);
        }

        [HttpPost]
        public bool UpdateTimezone([FromBody] UpdateTimezone vm)
        {
            return _expertService.UpdateTimezone(vm.UserId, vm.TimezoneId);
        }
    }
}