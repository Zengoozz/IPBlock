using API.Models;
using API.Repositories;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace API.Controllers
{
    [Route("api/ip")]
    [ApiController]
    public class IpController : ControllerBase
    {
        private readonly IPAddressLookupService _iPAddressLookupService;
        private readonly ApiFullResponsesRepository _apiFullResponsesRepository; //FOR TEST
        public IpController(
            IPAddressLookupService iPAddressLookupService,
            ApiFullResponsesRepository apiFullResponsesRepository //FOR TEST
            )
        {
            _iPAddressLookupService = iPAddressLookupService;
            _apiFullResponsesRepository = apiFullResponsesRepository; //FOR TEST
        }
        #region EndPoints
        [HttpPost("lookup")]
        public IActionResult FindCountryByIPAddress(string ipAddress = "")
        {
            if (ipAddress != "")
            {
                string pattern = "\\b( (25[0-5]|2[0-4][0-9]|1?[0-9]?[0-9])\\.\n(25[0-5]|2[0-4][0-9]|1?[0-9]?[0-9])\\.\n(25[0-5]|2[0-4][0-9]|1?[0-9]?[0-9])\\.\n(25[0-5]|2[0-4][0-9]|1?[0-9]?[0-9]) | ([A-Fa-f0-9]{1,4}:){1,7}[A-Fa-f0-9]{1,4} | :: )\\b";
                MatchCollection validIp = Regex.Matches(ipAddress, pattern);

                if (validIp.Count == 0) {
                    return BadRequest("Ip Address is invalid");
                }
            }

            ApiRequestModel request = new();

            request.IpAddress = ipAddress;

            ApiResponseFullModel response = _iPAddressLookupService.FetchAPIWithIpAddress(request);

            if(response.StatusCode == "200")
            {
                var apiResponse = response.ApiResponseData;
                return Ok(apiResponse);
            }
            else
            {
                return NotFound(response.ErrorMessage);
            }
        }
        [HttpGet("testGet")]
        public IActionResult GetAllGeoLocationResponsesSavedForTest() //FOR TEST
        {
            List<ApiResponseFullModel> response = _apiFullResponsesRepository.GetAll();

            return Ok(response);
        }
        #endregion
    }
}
