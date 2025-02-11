using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class ApiRequestModel
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string IpAddress { get; set; } = "";
        public bool HasDuration { get; set; } = false;
        public string Duration { get; set; } = "";

    }
}
