using Microsoft.AspNetCore.Mvc;
using stutor_core.Database;
using stutor_core.Models.Sql;
using stutor_core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CareerController : ControllerBase
    {
        private readonly CareerService _careerService;

        public CareerController(ApplicationDbContext db)
        {
            _careerService = new CareerService(db);
        }

        [HttpGet]
        public IEnumerable<AvailableJob> GetAllAvailableJobs()
        {
            return _careerService.GetAllAvailableJobs();
        }
    }
}
