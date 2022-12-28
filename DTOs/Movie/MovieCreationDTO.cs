using Microsoft.AspNetCore.Mvc;
using PopularMovieCatalogBackend.Helpers.CustomBinderMovies;
using System.ComponentModel.DataAnnotations;

namespace PopularMovieCatalogBackend.DTOs.Movie
{
    public class MovieCreationDTO
    {
        [StringLength(maximumLength: 80)]
        [Required]
        public string Title { get; set; }

        public bool InTheaters { get; set; }

        public string Trailer { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Summary { get; set; }        

        public IFormFile Poster { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GenresIds { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> MovieTheatersIds { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<MovieActorCreationDTO>>))]
        public List<MovieActorCreationDTO> Actors { get; set; }
    }
}
