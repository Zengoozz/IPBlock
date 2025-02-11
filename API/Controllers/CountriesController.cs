using API.Repositories;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/countries")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly BlockedCountriesRepository _blockedCountriesRepository;
        private readonly IPAddressLookupService _iPAddressLookupService;
        public CountriesController(
            BlockedCountriesRepository blockedCountriesRepository,
            IPAddressLookupService iPAddressLookupService
            )
        {
            _blockedCountriesRepository = blockedCountriesRepository;
            _iPAddressLookupService = iPAddressLookupService;
        }

        #region EndPoints
        [HttpGet("BlockedCountries")]
        public IActionResult? GetAllBlockedCountries([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, string countryCode = "", string countryName = "")
        {
            var sortedCountries = _blockedCountriesRepository.GetPaginatedBlockedCountries(pageNumber, pageSize, countryCode, countryName);

            return Ok(sortedCountries);
        }
        [HttpPost("Block")]
        public IActionResult? BlockCountryByCode(string countryCode)
        {
            var isSuccessResponse = _blockedCountriesRepository.Block(countryCode);

            if (isSuccessResponse == 1)
                return Ok(countryCode + " country blocked successfully");
            else if (isSuccessResponse == 2)
                return Conflict("Error: Temporal Duration Conflict");
            else if (isSuccessResponse == 3)
                return BadRequest(countryCode + " country is already blocekd");
            else
                return StatusCode(500, "Error in blocking");
        }
        [HttpDelete("Block")]
        public IActionResult? UnBlockCountryByCode(string countryCode)
        {
            var isSuccessResponse = _blockedCountriesRepository.UnBlock(countryCode);

            if (isSuccessResponse)
                return Ok(countryCode + " country unblocked successfully");
            else
                return NotFound("Country is already blocekd");
        }

        [HttpPost("Check-Block")]
        public IActionResult CheckIpCountryBlocked(string? iPAddress = null)
        {
            var apiResponse = _iPAddressLookupService.FetchAPIWithIpAddress(iPAddress);

            if (apiResponse.StatusCode == "200")
            {
                var blockedCountriesCodes = _blockedCountriesRepository.GetAllCountryCodesBlocked();

                var isBlocked = blockedCountriesCodes.FirstOrDefault(i => apiResponse.ApiDataModel != null && apiResponse.ApiDataModel.CountryCode2 == i);

                if (isBlocked == null)
                    return Ok(apiResponse.ApiDataModel?.IPAddress ?? "" + " is not blocked");

                return Ok(apiResponse.ApiDataModel?.IPAddress ?? "" + " is blocked");
            }
            else
            {
                return BadRequest(apiResponse.ErrorMessage);
            }

        }
        [HttpPost("BlockCurrentIPCountry")]
        public IActionResult BlockCurrentIPCountry(string? iPAddress = null)
        {
            var apiResponse = _iPAddressLookupService.FetchAPIWithIpAddress(iPAddress);

            if (apiResponse.StatusCode != "200" || apiResponse.ApiDataModel?.CountryCode2 == "")
                return StatusCode(500);

            var isSuccessResponse = _blockedCountriesRepository.Block(apiResponse.ApiDataModel?.CountryCode2 ?? "", apiResponse.ApiDataModel?.CountryName ?? "");

            if (isSuccessResponse == 1)
                return Ok(apiResponse.ApiDataModel?.CountryCode2 + " country blocked successfully");
            else if (isSuccessResponse == 2)
                return Conflict("Error: Temporal Duration Conflict");
            else if (isSuccessResponse == 3)
                return BadRequest(apiResponse.ApiDataModel?.CountryCode2 + " country is already blocekd");
            else
                return StatusCode(500, "Error in blocking");
        }

        [HttpPost("UnBlockCurrentIPCountry")]
        public IActionResult UnBlockCurrentIPCountry(string? iPAddress = null)
        {
            var apiResponse = _iPAddressLookupService.FetchAPIWithIpAddress(iPAddress);

            if (apiResponse.StatusCode != "200" || apiResponse.ApiDataModel?.CountryCode2 == "")
                return StatusCode(500);

            bool isSuccessResponse = _blockedCountriesRepository.UnBlock(apiResponse.ApiDataModel?.CountryCode2 ?? "");

            if (isSuccessResponse)
                return Ok(apiResponse.ApiDataModel?.CountryCode2 ?? "" + " country unblocked successfully");
            else
                return NotFound("Country is already unblocked");
        }

        #endregion
    }
}
