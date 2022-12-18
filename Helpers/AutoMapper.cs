using AutoMapper;
using PopularMovieCatalogBackend.DTOs.Actor;
using PopularMovieCatalogBackend.DTOs.Genre;
using PopularMovieCatalogBackend.Model;

namespace PopularMovieCatalogBackend.Helpers
{
    public class AutoMapper: Profile
    {
        public AutoMapper()
        {
            // Auto Mapper for Genre
            CreateMap<GenreDTO, Genre>().ReverseMap();
            CreateMap<GenreCreationDTO, Genre>();

            // Auto Mapper for Actor
            CreateMap<ActorDTO, Actor>().ReverseMap();
            CreateMap<ActorCreationDTO, Actor>()
                // Ignore the picture property from the ActorCreationDTO
            .ForMember(x => x.Picture, options => options.Ignore());
        }
    }
}
