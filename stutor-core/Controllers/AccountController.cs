using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using stutor_core.Database;
using stutor_core.Models.ViewModels;
using stutor_core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class AccountController : Controller
    {
        private ApplicationDbContext _db;

        public AccountController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public ActionResult ResendPhoneConfirmation([FromBody] string userId)
        {
            return Json(new { success = false, error = "Not Implemented" });
        }
    }
}
