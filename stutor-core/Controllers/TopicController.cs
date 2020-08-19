﻿using System;
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
    public class TopicController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> SubmitRequest(TopicRequest requestedTopic)
        {
            var userClaims = HttpContext.User.Claims;

            return Ok();
        }
    }
}