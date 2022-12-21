using PopularMovieCatalogBackend.Model.Movies;

namespace PopularMovieCatalogBackend.Model
{
    public class MoviesGenres
    {
        public int GenreId { get; set; }

        public int MovieId { get; set; }

        public Genre Genre { get; set; }

        public Movie Movie { get; set; }
    }
}
