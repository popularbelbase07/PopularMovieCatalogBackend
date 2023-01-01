﻿using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PopularMovieCatalogBackend.Entities.Movies;
using PopularMovieCatalogBackend.Model;
using PopularMovieCatalogBackend.Model.Movies;
using System.Diagnostics.CodeAnalysis;

namespace PopularMovieCatalogBackend
{
    // This is a central configuration class which contains Entity Framework for the application.
    // public class ApplicationDbContext : DbContext
    // With identity we can authenticate and authorize the user to the
    // different resources and build json web token that helps to user can easily authenticate
    public class ApplicationDbContext :IdentityDbContext
    {
        public ApplicationDbContext( [NotNullAttribute] DbContextOptions options) : base(options)
        {
        }
        // configure composite key for the many to many relationship of schema
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity< MoviesActors>()
                .HasKey(x => new { x.MovieId, x.ActorId });    

            modelBuilder.Entity<MoviesGenres>()
                .HasKey(x => new {x.MovieId, x.GenreId});

            modelBuilder.Entity<MovieTheatersMovies>()
                .HasKey(x => new { x.MovieTheaterId, x.MovieId });

            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Genre> Genres { get; set; }

        public DbSet<Actor>Actors { get; set; }

        public DbSet<MovieTheater> MoviesTheaters { get; set;}

        public DbSet<Movie> Movies { get; set; }

        public DbSet<MoviesActors> MoviesActors { get; set; }   

        public DbSet<MoviesGenres> MoviesGenres { get; set;}

        public DbSet<MovieTheatersMovies> MovieTheatersMovies { get; set; } 
        
        public DbSet<Rating> Ratings { get; set; }
    }
}
