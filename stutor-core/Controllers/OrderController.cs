using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using stutor_core.Database;
using stutor_core.Models.Sql;
using stutor_core.Services;

namespace stutor_core.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class OrderController : Controller
    {
        ApplicationDbContext _db;
        OrderService _repo;

        public OrderController(ApplicationDbContext db)
        {
            _db = db;
            _repo = new OrderService(_db);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitIntent(Order order)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitPasskeys(Order order)
        {
            //Check if order has already been validated
            //If not check to see if the passkeys are correct and mark the order as validated.
            return Ok();
        }

        [HttpPost]
        public JsonResult updateFeedback(Order incomingOrder)
        {
            JsonResult result = Json(new { error = ""});
            try
            {
                var rowsAffected = _repo.updateFeedback(incomingOrder);
                result = (rowsAffected > 0) ? Json(new { status = 200, error = "" }) : Json(new { status = 500, error = "" });
            }
            catch (System.Exception ex)
            {
                result = Json(new { status = 500, error = ex.Message });
            }
            return result;
        }

        [HttpPost]
        public IEnumerable<Order> GetAllByUserId([FromBody] string userId)
        {   
            return _repo.GetAllByUserId(userId);
        }
    }
}