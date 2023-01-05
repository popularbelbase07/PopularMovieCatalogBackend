﻿
using Microsoft.AspNetCore.Identity;
using PopularMovieCatalogBackend.Model.Movies;
using System.ComponentModel.DataAnnotations;

namespace PopularMovieCatalogBackend.Model
{
    public class Rating
    {
        public int Id { get; set; }
        [Range (1,5)]

        public int Rate { get; set; }

        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public string UserId { get; set; }

        public IdentityUser User { get; set; }  
    }
}
