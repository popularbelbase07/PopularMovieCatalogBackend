using System.ComponentModel.DataAnnotations;
using PopularMovieCatalogBackend.Validation;

namespace PopularMovieCatalogBackend.Model
{
    public class Genre
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="The field with name {0} is required")]
        [StringLength(50)]
        [FirstLetterUpperCaseAttribution]
        public string Name { get; set; }
    }
}
