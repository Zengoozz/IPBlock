using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class ApiResponseFullModel
    {
        public Guid Id { get; } = Guid.NewGuid();
        public bool IsSuccess { get; set; } = false;
        public string StatusCode { get; set; } = "";
        public string ErrorMessage { get; set; } = "";

        [ForeignKey("ApiRequest")]
        public required Guid ApiRequestId { get; set; }
        public ApiRequestModel ApiRequest { get; set; } = new ApiRequestModel();
        public ApiResponseDataModel? ApiResponseData { get; set; } = null;
    }
}
