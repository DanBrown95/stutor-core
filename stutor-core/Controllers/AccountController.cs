using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using stutor_core.Configurations;
using stutor_core.Database;
using stutor_core.Models.ViewModels;
using stutor_core.Services;
using stutor_core.Services.Controllers;
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
        private SMSSettings _smsSettings;

        public AccountController(ApplicationDbContext db, IOptions<SMSSettings> smsSettings)
        {
            _db = db;
            _smsSettings = smsSettings.Value;
        }

        [HttpPost]
        public async Task<JsonResult> ResendPhoneConfirmation([FromBody] string userId)
        {
            var smsService = new SmsService(_smsSettings);
            var userPhone = _db.User.FirstOrDefault(u => u.Id == userId).Phone;

            var status = await smsService.SendVerificationAsync(userPhone);
            return Json(new { status = status });
        }

        [HttpPost]
        public async Task<JsonResult> VerifyPhonePin([FromBody] VerifyPhonePinVM vm)
        {
            var smsService = new SmsService(_smsSettings);
            var userPhone = _db.User.FirstOrDefault(u => u.Id == vm.UserId).Phone;
            if (userPhone == null || userPhone == "")
            {
                return Json(new { success = false, error = "Invalid Id or no number on file." });
            }

            try
            {
                var result = await smsService.VerifyConfirmationPin(userPhone, vm.Pin);
                if (result == "approved")
                {
                    var user = _db.User.FirstOrDefault(x => x.Id == vm.UserId);
                    user.Phone_Verified = true;
                    _db.Update<stutor_core.Models.Sql.User>(user);
                    var rowsAffected = _db.SaveChanges();

                    if (rowsAffected == 1)
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        // tbi: log that either we were unable to update the user or more than 1 user's phone status was updated.
                    }
                }
                return Json(new { success = false, exception = false, error = "Pin is incorrect" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, exception = true, error = ex.Message });
            }
            
        }

    }
}
