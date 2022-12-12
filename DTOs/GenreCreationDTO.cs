using PopularMovieCatalogBackend.Validation;
using System.ComponentModel.DataAnnotations;

namespace PopularMovieCatalogBackend.DTOs
{
    public class GenreCreationDTO
    {
        [Required(ErrorMessage = "The field with name {0} is required")]
        [StringLength(10)]
        [FirstLetterUpperCaseAttribution]
        public string Name { get; set; }
    }
}
