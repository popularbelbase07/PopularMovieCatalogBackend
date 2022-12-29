namespace PopularMovieCatalogBackend.DTOs.Movie
{
    public class LandingPageDTO
    {
        public List<MovieDTO> InTheaters { get; set; }
        public List<MovieDTO> UpcommingReleases { get; set; }

    }
}
