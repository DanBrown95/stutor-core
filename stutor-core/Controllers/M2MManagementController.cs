using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using stutor_core.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Controllers
{
    [Route("api/verification/[action]")]
    [Authorize]
    [ApiController]
    public class M2MManagementController : Controller
    {
        private readonly StutorCoreM2MSettings _m2mConfig;

        public M2MManagementController(StutorCoreM2MSettings config)
        {
            _m2mConfig = config;
        }

        private string ApiToken
        {
            get
            {
                var client = new RestClient($"https://{_m2mConfig.Domain}/oauth/token");
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("applicatioin/x-www-form-urlencoded", $"grant_type=client_credentials&client_id={_m2mConfig.ClientId}&client_secret={_m2mConfig.ClientSecret}&audience={_m2mConfig.Audience}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                var result = JsonConvert.DeserializeObject<OauthAccessToken>(response.Content);
                return result.AccessToken;
            }
        }

        private ManagementApiClient Auth0ManagementClient
        {
            get
            {
                return new ManagementApiClient(ApiToken, new Uri($"https://{_m2mConfig.Domain}/api/v2"));
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ResendEmailVerification([FromBody] string userId)
        {
            try
            {
                var verifyEmailJobRequest = new VerifyEmailJobRequest
                {
                    UserId = userId
                };
                await Auth0ManagementClient.Jobs.SendVerificationEmailAsync(verifyEmailJobRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(new { success = true });
        }

    }
}
