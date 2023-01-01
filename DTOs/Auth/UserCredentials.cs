using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PopularMovieCatalogBackend.DTOs.Auth
{
    public class UserCredentials
    {
        [Required]
        [EmailAddress]
        [JsonProperty("email")]
        public string Email { get; set; } 
        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
