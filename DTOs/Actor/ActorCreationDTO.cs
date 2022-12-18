using System.ComponentModel.DataAnnotations;

namespace PopularMovieCatalogBackend.DTOs.Actor
{
    public class ActorCreationDTO
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Biography { get; set; }
        // reciving the picture from the client side as a input
        public IFormFile Picture { get; set; }
    }
}
