using PopularMovieCatalogBackend.DTOs.Actor;
using PopularMovieCatalogBackend.DTOs.Genre;
using PopularMovieCatalogBackend.DTOs.MovieTheater;

namespace PopularMovieCatalogBackend.DTOs.Movie
{
    public class MoviePutGetDTO
    {
        public MovieDTO  Movie { get; set; }

        public List<GenreDTO> SelectedGenres { get; set; }

        public List<GenreDTO> NonSelectedGenres { get; set; }

        public List<MovieTheaterDTO> SelectedMovieTheater { get; set; }

        public List<MovieTheaterDTO> NonSelectedMovieTheater { get; set; }

        public List<ActorsMovieDTO> Actors { get; set; }
    }
}
