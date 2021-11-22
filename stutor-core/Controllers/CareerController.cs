using Microsoft.AspNetCore.Mvc;
using stutor_core.Models.Sql;
using stutor_core.Services.Interfaces;
using System.Collections.Generic;

namespace stutor_core.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CareerController : ControllerBase
    {
        private readonly ICareerService _careerService;

        public CareerController(ICareerService careerService)
        {
            _careerService = careerService;
        }

        [HttpGet]
        public IEnumerable<AvailableJob> GetAllAvailableJobs()
        {
            return _careerService.GetAllAvailableJobs();
        }
    }
}
