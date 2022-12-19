using AutoMapper;
using NetTopologySuite.Geometries;
using PopularMovieCatalogBackend.DTOs.Actor;
using PopularMovieCatalogBackend.DTOs.Genre;
using PopularMovieCatalogBackend.DTOs.MovieTheater;
using PopularMovieCatalogBackend.Model;

namespace PopularMovieCatalogBackend.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            // Auto Mapper for Genre
            CreateMap<GenreDTO, Genre>().ReverseMap();
            CreateMap<GenreCreationDTO, Genre>();

            // Auto Mapper for Actor
            CreateMap<ActorDTO, Actor>().ReverseMap();
            CreateMap<ActorCreationDTO, Actor>()
                // Ignore the picture property from the ActorCreationDTO
            .ForMember(x => x.Picture, options => options.Ignore());

            //Auto Mapper for MovieTheater
            CreateMap<MovieTheater, MovieTheaterDTO>()
                .ForMember(x => x.Latitude, dto => dto.MapFrom(prop => prop.Location.Y))
                .ForMember(x => x.Longitude, dto => dto.MapFrom(prop => prop.Location.X));
            CreateMap<MovieTheaterCreationDTO, MovieTheater>()
                .ForMember(x => x.Location, x => x.MapFrom(dto => geometryFactory.CreatePoint(new Coordinate(dto.Longitude, dto.Latitude))));
        }
    }
}
