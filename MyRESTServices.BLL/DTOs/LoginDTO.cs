using System.Text.Json.Serialization;

namespace MyRESTServices.BLL.DTOs
{
    public class LoginDTO
    {
        public string? Username { get; set; }

        [JsonIgnore]
        public string? Password { get; set; }
        public string? Token { get; set; }

        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; } = string.Empty;
    }
}
