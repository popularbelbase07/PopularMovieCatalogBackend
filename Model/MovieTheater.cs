using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace PopularMovieCatalogBackend.Model
{
    public class MovieTheater
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 75)]
        public string Name { get; set; }
        //For location install the SQLSERVER/NETTOPOLOGYSUITE
        public Point Location { get; set; }
    }
}
