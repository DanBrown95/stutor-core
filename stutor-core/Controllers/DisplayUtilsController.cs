using Microsoft.AspNetCore.Mvc;
using stutor_core.Models.Sql;
using System.Collections.Generic;
using stutor_core.Services.Interfaces;

namespace stutor_core.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DisplayUtilsController : Controller
    {

        private ILocationService _timezoneService;
        
        public DisplayUtilsController(ILocationService timezoneService)
        {
            _timezoneService = timezoneService;
        }

        [HttpGet]
        public IEnumerable<Timezone> Timezones()
        {
            return _timezoneService.GetAll();
        }

    }
}
