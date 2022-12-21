using System.ComponentModel.DataAnnotations;

namespace PopularMovieCatalogBackend.DTOs.MovieTheater
{
    public class MovieTheaterDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
       
        public double Latitude { get; set; }
        
        public double Longitude { get; set; }
    }
}
