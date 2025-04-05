using CinemaApp.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaApp.Data.Configuration
{
    internal class CinemaMovieConfiguration : IEntityTypeConfiguration<CinemaMovie>
    {
        public void Configure(EntityTypeBuilder<CinemaMovie> entity)
        {
            
            entity
                .HasKey(cm => cm.Id);

            entity
                .Property(cm => cm.MovieId)
                .IsRequired();

            entity
                .Property(cm => cm.CinemaId)
                .IsRequired();

            entity
                .Property(cm => cm.AvailableTickets)
                .IsRequired();
            
            entity
                .HasIndex(cm => new { cm.CinemaId, cm.MovieId})
                .IsUnique();
            
            entity
                .Property(cm => cm.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            entity
                .Property(cm => cm.Showtimes)
                .IsRequired(false)
                .IsUnicode(false)
                .HasMaxLength(5)
                .HasDefaultValue("00000");

            entity
                .HasOne(cm => cm.Cinema)
                .WithMany(cm => cm.CinemaMovies)
                .HasForeignKey(cm => cm.CinemaId)
                .OnDelete(DeleteBehavior.NoAction);

            entity
                .HasOne(cm => cm.Movie)
                .WithMany(cm => cm.MovieCinemas)
                .HasForeignKey(cm => cm.MovieId)
                .OnDelete(DeleteBehavior.NoAction);

            entity
                .HasQueryFilter(cm => cm.IsDeleted == false);

        }
    }
}
