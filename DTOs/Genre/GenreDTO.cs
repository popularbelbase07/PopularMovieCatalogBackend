using PopularMovieCatalogBackend.Validation;
using System.ComponentModel.DataAnnotations;

namespace PopularMovieCatalogBackend.DTOs.Genre
{
    public class GenreDTO
    {

        public int Id { get; set; }

        public string Name { get; set; }
    }
}
