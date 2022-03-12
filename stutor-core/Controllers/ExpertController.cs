using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using stutor_core.Configurations;
using stutor_core.Models;
using stutor_core.Models.Sql;
using stutor_core.Models.ViewModels;
using stutor_core.Services;
using stutor_core.Services.Interfaces;

namespace stutor_core.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExpertController : Controller
    {
        private IExpertService _expertService;
        private IOrderService _orderService;
        private ILocationService _locationService;
        private AWSS3Service _awsS3Service;

        public ExpertController(AWSS3Settings S3Settings, IExpertService expertService, IOrderService orderService, ILocationService locationService)
        {
            _expertService = expertService;
            _orderService = orderService;
            _locationService = locationService;
            _awsS3Service = new AWSS3Service(S3Settings);
        }

        [HttpPost]
        public async Task<JsonResult> UploadDocuments(IFormCollection documents)
        {
            if(documents.Files.Count < 1)
            {
                return Json(new { status = 400 });
            }

            try
            {
                var result = await _awsS3Service.UploadMultipleToBucketAsync(documents);
            }
            catch (Exception)
            {
                return Json(new { status = 500 });
            }
            
            return Json(new { status = 200 });
        }

        [HttpPost]
        public int Register(ExpertRegistrationVM formData)
        {
            var application = new ExpertApplication{
                UserId = formData.UserId,
                TopicId = formData.TopicId,
                Address = formData.Location.Address,
                Longitude = formData.Location.Coords.Lng,
                Latitude = formData.Location.Coords.Lat,
                WebsiteUrl = formData.WebsiteUrl,
                LinkedinUrl = formData.LinkedinUrl,
                Certifications = formData.Certifications,
                YearsOfExperience = formData.YearsOfExperience,
                Notes = formData.Notes,
                Specialties = formData.Specialties
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
        public LocationData ExpertLocation([FromBody] string userId)
        {
            return _locationService.GetLocationByUserId(userId);
        }

        [HttpPost]
        public IEnumerable<Topic> TopicsByUserId([FromBody] string userId)
        {
            return _expertService.GetExpertTopicsByUserId(userId);
        }

        [HttpPost]
        public IEnumerable<Order> OrdersByUserId([FromBody] string userId)
        {
            return _orderService.GetExpertOrdersByUserId(userId);
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
        public bool UpdateLocation([FromBody] UpdateLocation vm)
        {
            return _expertService.UpdateLocation(vm.UserId, vm.location);
        }

        [HttpPost]
        public JsonResult RevokeTopicExpert([FromBody] RevokeTopicExpert vm)
        {
            var result = _expertService.RevokeTopicExpert(vm.TopicExpertId, vm.ExpertId);
            return Json(new { success = result });
        }

        [HttpPost]
        public IEnumerable<Specialty> GetSpecialties([FromBody] int topicExpertId)
        {
            return _expertService.GetSpecialties(topicExpertId);
        }

        [HttpPost]
        public JsonResult UpdateTopicExpertSpecialties([FromBody] UpdateTopicExpertSpecialties vm)
        {
            bool result = _expertService.UpdateTopicExpertSpecialties(vm.TopicExpertId, vm.SpecialtyIds);
            return Json(new { success = result });
        }

        [HttpPost]
        public bool HasIncompleteOrders([FromBody] string userId)
        {
            return _expertService.HasIncompleteOrders(userId);
        }
    }
}