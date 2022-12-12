using PopularMovieCatalogBackend.Validation;
using System.ComponentModel.DataAnnotations;

namespace PopularMovieCatalogBackend.DTOs
{
    public class GenreDTO
    {

        public int Id { get; set; }
      
        public string Name { get; set; }
    }
}
