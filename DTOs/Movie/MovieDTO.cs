﻿using PopularMovieCatalogBackend.DTOs.Actor;
using PopularMovieCatalogBackend.DTOs.Genre;
using PopularMovieCatalogBackend.DTOs.MovieTheater;

namespace PopularMovieCatalogBackend.DTOs.Movie
{
    public class MovieDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string Trailer { get; set; }

        public bool InTheaters { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Poster { get; set; }

        public List<GenreDTO> Genres { get; set; }

        public List<MovieTheaterDTO> MovieTheaters { get; set; }

        public List<ActorsMovieDTO> Actors { get; set; }

        //for rating
        public double AverageVote { get; set; }

        public int UserVote { get; set; }
    }
}
