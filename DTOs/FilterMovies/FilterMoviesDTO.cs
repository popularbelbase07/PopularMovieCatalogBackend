using PopularMovieCatalogBackend.DTOs.Genre;

namespace PopularMovieCatalogBackend.DTOs.FilterMovies
{
    public class FilterMoviesDTO
    {
        public int Page { get; set; }

        public int RecordPerPage { get; set; }

        public PaginationDTO PaginationDTO { get
            {
                return new PaginationDTO()
                { Page = Page, RecordsPerPage = RecordPerPage };           
            }
        }
        public string Title { get; set; }

        public int GenreId { get; set; }

        public bool InTheaters { get; set; }

        public bool UpcommingReleases { get; set; }

    }
}
