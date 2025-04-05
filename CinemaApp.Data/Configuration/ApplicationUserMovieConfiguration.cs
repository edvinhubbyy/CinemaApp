﻿using CinemaApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaApp.Data.Configuration
{
    internal class ApplicationUserMovieConfiguration : IEntityTypeConfiguration<ApplicationUserMovie>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserMovie> entity)
        {
            // Define the composite primary key of entity 
            entity
                .HasKey(aum => new { aum.ApplicationUserId, aum.MovieId });

            // Set default value of IsDeleted proparty
            entity
                .Property(aum => aum.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            // Define the relation between the Application use movie and application user
            entity
                .HasOne(aum => aum.ApplicationUser)
                .WithMany(au => au.Watchlist)
                .HasForeignKey(aum => aum.ApplicationUserId)
                .OnDelete(DeleteBehavior.NoAction);

            
            entity
                .HasOne(aum => aum.Movie)
                .WithMany(m => m.MovieApplicationUsers)
                .HasForeignKey(aum => aum.MovieId)
                .OnDelete(DeleteBehavior.NoAction);


            // Ensure that only existing records are used in the business logic
            entity
                .HasQueryFilter(aum => aum.IsDeleted == false);

        }
    }
}
