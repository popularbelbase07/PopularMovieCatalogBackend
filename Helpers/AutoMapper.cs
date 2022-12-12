using AutoMapper;
using PopularMovieCatalogBackend.DTOs;
using PopularMovieCatalogBackend.Model;

namespace PopularMovieCatalogBackend.Helpers
{
    public class AutoMapper: Profile
    {
        public AutoMapper()
        {
            CreateMap<GenreDTO, Genre>().ReverseMap();
            CreateMap<GenreCreationDTO, Genre>();
        }
    }
}
