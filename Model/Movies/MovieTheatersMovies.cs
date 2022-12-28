using PopularMovieCatalogBackend.Model;
using PopularMovieCatalogBackend.Model.Movies;

namespace PopularMovieCatalogBackend.Entities.Movies
{
    public class MovieTheatersMovies
    {
        public int MovieTheaterId { get; set; } 

        public int MovieId { get; set; } 
        
        public MovieTheater MovieTheater { get; set; }

        public Movie Movie { get; set; }


    }
}
