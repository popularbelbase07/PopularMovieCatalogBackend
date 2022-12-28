using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using NetTopologySuite.Geometries;
using PopularMovieCatalogBackend.DTOs.Actor;
using PopularMovieCatalogBackend.DTOs.Genre;
using PopularMovieCatalogBackend.DTOs.Movie;
using PopularMovieCatalogBackend.DTOs.MovieTheater;
using PopularMovieCatalogBackend.Entities.Movies;
using PopularMovieCatalogBackend.Model;
using PopularMovieCatalogBackend.Model.Movies;

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


            //Auto Mapper for Movie

                 CreateMap<MovieCreationDTO, Movie>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.MoviesGenres, options => options.MapFrom(MapMoviesGenres))
                .ForMember(x => x.MovieTheatersMovies, options => options.MapFrom(MapMovieTheatersMovies))
                .ForMember(x => x.MoviesActors, options => options.MapFrom(MapMoviesActors));

            // Auto Mapper for movies by id
            CreateMap<Movie, MovieDTO>()
                .ForMember(x => x.Genres, option => option.MapFrom(MapSpecificMoviesGenres))
                .ForMember(x => x.MovieTheaters, option => option.MapFrom(MapSpecificMovieTheatersMovies))
                .ForMember(x => x.Actors, option => option.MapFrom(MapSpecificMoviesActors));

        }
        // All three following private methods are for CREATE MOVIE

        private List<MoviesGenres> MapMoviesGenres(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MoviesGenres>();
            if (movieCreationDTO.GenresIds == null)
            {
                return result;
            }
            foreach (var id in movieCreationDTO.GenresIds)
            {
                result.Add(new MoviesGenres() { GenreId = id });
            }
            return result;
        }
        

         private List<MovieTheatersMovies> MapMovieTheatersMovies(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MovieTheatersMovies>();

            if (movieCreationDTO.MovieTheatersIds == null) { return result; }

            foreach (var id in movieCreationDTO.MovieTheatersIds)
            {
                result.Add(new MovieTheatersMovies() { MovieTheaterId = id});
            }
            return result;
        }


        private List<MoviesActors> MapMoviesActors(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MoviesActors>();

            if (movieCreationDTO.Actors == null) { return result; }

            foreach (var actor in movieCreationDTO.Actors)
            {
                result.Add(new MoviesActors() { ActorId = actor.Id, Character = actor.Character });
            }
            return result;
        }



        // All three followings methods for find by id (movies/3) http call

        private List<GenreDTO> MapSpecificMoviesGenres(Movie movie, MovieDTO movieDTO)
        {
            var result = new List<GenreDTO>();

            if (movie.MoviesGenres != null)
            {
                foreach (var genre in movie.MoviesGenres)
                {
                    result.Add(new GenreDTO() { Id = genre.GenreId, Name = genre.Genre.Name });
                }
            }
            return result;

        }

        private List<MovieTheaterDTO> MapSpecificMovieTheatersMovies(Movie movie, MovieDTO movieDTO)
        {
            var result = new List<MovieTheaterDTO>();

            if (movie.MovieTheatersMovies != null)
            {
                foreach (var movieTheatersMovies in movie.MovieTheatersMovies)
                {
                    result.Add(new MovieTheaterDTO()
                    {
                        Id = movieTheatersMovies.MovieTheaterId,
                        Name = movieTheatersMovies.MovieTheater.Name,
                        Latitude = movieTheatersMovies.MovieTheater.Location.Y,
                        Longitude = movieTheatersMovies.MovieTheater.Location.X
                    });
                }
            }
            return result;
        }

        private List<DTOs.Movie.ActorsMovieDTO> MapSpecificMoviesActors(Movie movie, MovieDTO movieDTO)
        {
            var result = new List<DTOs.Movie.ActorsMovieDTO>();

            if (movie.MoviesActors != null)
            {
                foreach (var moviesActor in movie.MoviesActors)
                {
                    result.Add(new DTOs.Movie.ActorsMovieDTO()
                    {
                        Id = moviesActor.ActorId,
                        Name = moviesActor.Actor.Name,
                        Character = moviesActor.Character,
                        Picture = moviesActor.Actor.Picture,
                        Order = moviesActor.Order,
                    });
                }
            }

            return result;
        }


    }
}


public class MyProfile : Profile
{
   public MyProfile()
   {

        CreateMap<MovieCreationDTO, Movie>();
   }
}