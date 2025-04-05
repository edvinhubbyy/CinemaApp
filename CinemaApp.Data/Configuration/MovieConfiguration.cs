using CinemaApp.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaApp.Data.Configuration
{
    internal class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> entity)
        {
            entity
                .HasKey(m => m.Id);

            entity
                .Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(150);

            entity
                .Property(m => m.Genre)
                .IsRequired()
                .HasMaxLength(30);

            entity
                .Property(m => m.ReleaseDate)
                .IsRequired();

            entity
                .Property(m => m.Director)
                .IsRequired()
                .HasMaxLength(150);

            entity
                .Property(m => m.Duration)
                .IsRequired();

            entity
                .Property(m => m.Description)
                .IsRequired()
                .HasMaxLength(1024);
            
            entity
                .Property(m => m.ImageUrl)
                .IsRequired(false)
                .HasMaxLength(2048);

            entity
                .Property(m => m.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);


            entity
                .HasQueryFilter(m => m.IsDeleted == false);
            



        }
    }
}
