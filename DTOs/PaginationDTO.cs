namespace PopularMovieCatalogBackend.DTOs
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;
        private int recordPerPage = 10; 
        private readonly int maxmiumRecordPerPage = 50;



        public int RecordsPerPage {
            get
            { return recordPerPage; }
            set
            {
                recordPerPage = (value>maxmiumRecordPerPage) ? value : maxmiumRecordPerPage;
            }
        }
    }
}
