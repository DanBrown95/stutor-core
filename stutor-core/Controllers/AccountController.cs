using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;
using stutor_core.Configurations;
using stutor_core.Database;
using stutor_core.Models.Sql;
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
        private UserService _userService;

        public AccountController(ApplicationDbContext db, IOptions<SMSSettings> smsSettings)
        {
            _db = db;
            _smsSettings = smsSettings.Value;
            _userService = new UserService(_db);
        }

        [HttpPost]
        public async Task<JsonResult> ResendPhoneConfirmation([FromBody] string userId)
        {
            var smsService = new SmsService(_smsSettings);
            string status;
            string userPhone;

            try
            {
                userPhone = _db.User.FirstOrDefault(u => u.Id == userId).Phone;
            }
            catch (Exception)
            {
                Log.Error("Could not pull phone field from user {UserId} to resend phone confirmation text", userId);
                return Json(new { status = "fail", error = "Invalid User ID" });
            }

            try
            {
                status = await smsService.SendVerificationAsync(userPhone);
            }
            catch (Exception)
            {
                Log.Error("Could not resend phone verification text to {UserId} phone {Phone}", userId, userPhone);
                return Json(new { status = "fail", error = "Error with sms service" });
            }

            return Json(new { status = status });
        }

        [HttpPost]
        public async Task<JsonResult> VerifyPhonePin([FromBody] VerifyPhonePinVM vm)
        {
            var smsService = new SmsService(_smsSettings);
            User user = null; 

            try
            {
                user = _db.User.FirstOrDefault(u => u.Id == vm.UserId);
                if (user == null || user.Phone == null || user.Phone == "")
                {
                    Log.Error("Could not find phone for user with id {userId} to verify phone pin", vm.UserId);
                    return Json(new { success = false, error = "Invalid Id or no number on file." });
                }

                var result = await smsService.VerifyConfirmationPin(user.Phone, vm.Pin);
                if (result == "approved")
                {
                    user.Phone_Verified = true;
                    _db.Update<User>(user);
                    var rowsAffected = _db.SaveChanges();

                    if (rowsAffected == 1)
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        Log.Error("< or > 1 user had their phone_verified status set to true when updating userId {userId}.", vm.UserId);
                    }
                }
                return Json(new { success = false, exception = false, error = "Pin is incorrect" });
            }
            catch (Exception ex)
            {
                Log.Error("Error thrown verifying phone confirmation pin with phone {phone} and pin {pin}", user.Phone, vm.Pin);
                return Json(new { success = false, exception = true, error = ex.Message });
            }
            
        }

        [HttpPost]
        public JsonResult UpdatePhoneNumber([FromBody] UpdatePhoneVM vm)
        {
            var updated = _userService.UpdatePhoneNumber(vm.UserId, vm.OldPhone, vm.NewPhone);

            return Json(new { success = updated });
        }
    }
}
