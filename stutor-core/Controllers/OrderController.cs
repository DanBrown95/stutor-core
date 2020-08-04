using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using stutor_core.Models;

namespace stutor_core.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : Controller
    {
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
    }
}