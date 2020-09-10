using Microsoft.AspNetCore.Mvc;
using stutor_core.Database;
using stutor_core.Models.Sql;
using stutor_core.Services;
using System.Collections.Generic;

namespace stutor_core.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DisplayUtilsController : Controller
    {

        private TimezoneService _timezoneService;
        
        public DisplayUtilsController(ApplicationDbContext db)
        {
            _timezoneService = new TimezoneService(db);
        }

        [HttpGet]
        public IEnumerable<Timezone> Timezones()
        {
            return _timezoneService.GetAll();
        }

    }
}
