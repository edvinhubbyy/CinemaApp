using CinemaApp.Data.Models;
using CinemaApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Data
{
    public class CinemaDbContext : IdentityDbContext<ApplicationUser,IdentityRole<Guid>,Guid>
    {
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; } = null!;

        public DbSet<Cinema> Cinemas { get; set; } = null!;

        public DbSet<CinemaMovie> CinemasMovies { get; set; } = null!;

        public DbSet<ApplicationUserMovie> UsersMovies { get; set; } = null!;

        public DbSet<Ticket> Tickets { get; set; } = null!;

    }
}