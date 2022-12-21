namespace PopularMovieCatalogBackend.Model.Movies
{
    public class MovieTheatersMovies
    {
        public int  MovieTheatersId { get; set; }   
        
        public int MovieId { get; set; }

        public MovieTheater MovieTheater { get; set; }

        public Movie Movie { get; set; }

    }
}
