using System.ComponentModel.DataAnnotations;

namespace PopularMovieCatalogBackend.Model.Movies
{
    public class Movie
    {
        public int Id { get; set; }

        [StringLength(maximumLength: 80)]
        [Required]
        public string Title { get; set; } 

        public string Summary { get; set; }

        public string Trailer { get; set; }

        public bool InTheaters { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Poster { get; set; }

        // Many to many Relationship
        public List<MoviesGenres> ?MoviesGenres { get; set; }

        public List<MovieTheatersMovies> ?MovieTheatersMovies { get; }

        public List<MoviesActors> ?MoviesActors { get; set; }


    }
}
