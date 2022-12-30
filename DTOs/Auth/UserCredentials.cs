using System.ComponentModel.DataAnnotations;

namespace PopularMovieCatalogBackend.DTOs.Auth
{
    public class UserCredentials
    {        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]  

        public string Password { get; set; }
    }
}
