using System.ComponentModel.DataAnnotations;

namespace PopularMovieCatalogBackend.Model.Movies
{
    public class MoviesActors
    {
        public int MovieId { get; set; }

        public int ActorId { get; set; }

        [StringLength(maximumLength:80)]
        public string Character { get; set; }

        public int Order { get; set; }

        public Movie Movie { get; set; }  
        
        public Actor Actor { get; set; }

    }
}
