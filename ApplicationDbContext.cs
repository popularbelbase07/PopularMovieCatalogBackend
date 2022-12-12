using Microsoft.EntityFrameworkCore;
using PopularMovieCatalogBackend.Model;
using System.Diagnostics.CodeAnalysis;

namespace PopularMovieCatalogBackend
{
    // This is a central configuration class which contains Entity Framework for the application.

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext( [NotNullAttribute] DbContextOptions options) : base(options)
        {
        }

        public DbSet<Genre> Genres { get; set; }
    }
}
