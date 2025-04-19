using CinemaApp.Data.Models;
using CinemaApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CinemaApp.Data
{
    public class CinemaDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        // This constructor is introduced for debugging purposes
        public CinemaDbContext()
        {

        }

        public CinemaDbContext(DbContextOptions<CinemaDbContext> options)
            : base(options)
        {
        }

        // EF Core will take care of loading the DbSet<T>
        public DbSet<ApplicationUserMovie> ApplicationUserMovies { get; set; } = null!;

        public DbSet<Cinema> Cinemas { get; set; } = null!;

        public DbSet<CinemaMovie> CinemasMovies { get; set; } = null!;

        public DbSet<Movie> Movies { get; set; } = null!;

        public DbSet<Ticket> Tickets { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}