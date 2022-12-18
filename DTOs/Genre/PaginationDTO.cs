namespace PopularMovieCatalogBackend.DTOs.Genre
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;
        private int recordsPerPage = 10;
        private readonly int maxmiumRecordsPerPage = 50;



        public int RecordsPerPage
        {
            get
            { return recordsPerPage; }
            set
            {
                recordsPerPage = value > maxmiumRecordsPerPage ? maxmiumRecordsPerPage : value;
            }
        }
    }
}
