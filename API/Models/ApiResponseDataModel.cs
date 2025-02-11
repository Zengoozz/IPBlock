namespace API.Models
{
    public class ApiResponseDataModel
    {
        public Guid Id { get; } = Guid.NewGuid(); 
        public string IPAddress { get; set; } = "";
        public string Hostname { get; set; } = "";
        public string ContinentCode { get; set; } = "";
        public string ContinentName { get; set; } = "";
        public string CountryCode2 { get; set; } = "";
        public string CountryCode3 { get; set; } = "";
        public string CountryName { get; set; } = "";
        public string CountryCapital { get; set; } = "";
        public string StateProv { get; set; } = "";
        public string District { get; set; } = "";
        public string City { get; set; } = "";
        public string Languages { get; set; } = "";
        public string ISP { get; set; } = "";
        public string Organization { get; set; } = "";
    }
}
