using PopularMovieCatalogBackend.DTOs.Genre;
using PopularMovieCatalogBackend.DTOs.MovieTheater;

namespace PopularMovieCatalogBackend.DTOs.Movie
{
    public class MoviePostGetDTO
    {

        public List<GenreDTO> Genres{ get; set; }

        public List<MovieTheaterDTO> MovieTheaters{ get; set; } 

    }
}
