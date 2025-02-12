using API.Models;
using API.Repositories;
using IPGeolocation;

namespace API.Services
{
    public class IPAddressLookupService
    {
        private readonly IPGeolocationAPI _iPGeolocationAPI;
        private readonly ApiFullResponsesRepository _apiFullResponsesRepository;

        public IPAddressLookupService(
            IPGeolocationAPI iPGeolocationAPI,
            ApiFullResponsesRepository apiFullResponsesRepository
            )
        {
            _iPGeolocationAPI = iPGeolocationAPI;
            _apiFullResponsesRepository = apiFullResponsesRepository;
        }

        public ApiResponseFullModel FetchAPIWithIpAddress(ApiRequestModel request)
        {
            try
            {
                Dictionary<string, object> geoLocation = new Dictionary<string, object>();
                ApiResponseFullModel responseModel = new()
                {
                    ApiRequestId = request.Id,
                    ApiRequest = request,
                };

                // If there's no ipAddress provided
                if (request.IpAddress == "")
                {
                    geoLocation = _iPGeolocationAPI.GetGeolocation();
                }
                else
                {
                    GeolocationParams geoParams = new GeolocationParams();
                    geoParams.SetIPAddress(request.IpAddress);

                    geoLocation = _iPGeolocationAPI.GetGeolocation(geoParams);
                }

                string status = geoLocation.ContainsKey("status") ?
                    geoLocation["status"]?.ToString() ?? "" : "Unknown";


                responseModel.StatusCode = status;

                if (status == "200")
                {
                    responseModel.IsSuccess = true;
                    responseModel.ErrorMessage = "";
                    responseModel.ApiResponseData = ConvertToGeoLocationResponseModel((Geolocation)geoLocation["response"]);
                }
                else
                {
                    responseModel.IsSuccess = false;
                    responseModel.ErrorMessage = geoLocation["message"].ToString() ?? "";
                    responseModel.ApiResponseData = null;
                }

                return responseModel;
            }
            catch (Exception)
            {
                throw;
            }

        }

        private ApiResponseDataModel ConvertToGeoLocationResponseModel(Geolocation model)
        {
            ApiResponseDataModel geoLocationResponseModel = new ApiResponseDataModel();
            geoLocationResponseModel.IPAddress = model.GetIPAddress();
            geoLocationResponseModel.Hostname = model.GetHostname();
            geoLocationResponseModel.ContinentCode = model.GetContinentCode();
            geoLocationResponseModel.ContinentName = model.GetContinentName();
            geoLocationResponseModel.CountryCode2 = model.GetCountryCode2();
            geoLocationResponseModel.CountryCode3 = model.GetCountryCode3();
            geoLocationResponseModel.CountryName = model.GetCountryName();
            geoLocationResponseModel.StateProv = model.GetStateProvince();
            geoLocationResponseModel.District = model.GetDistrict();
            geoLocationResponseModel.City = model.GetCity();
            geoLocationResponseModel.Languages = model.GetLanguages();
            geoLocationResponseModel.ISP = model.GetISP();
            geoLocationResponseModel.Organization = model.GetOrganization();

            return geoLocationResponseModel;
        }
    }
}
