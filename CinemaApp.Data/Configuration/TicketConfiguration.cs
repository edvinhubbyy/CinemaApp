﻿namespace CinemaApp.Data.Configuration
{
    using Models;
    using Common.Constants;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using CinemaApp.Models;
    using System.Reflection.Emit;

    internal class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> entity)
        {
            // Define the primary key of the Ticket entity
            entity
                .HasKey(t => t.Id);

            // Define constraints for the Price column
            entity
                .Property(t => t.Price)
                .IsRequired()
                .HasColumnType(EntityConstants.MoneyType);

            // Define constraints for the CinemaMovieId column
            entity
                .Property(t => t.CinemaMovieId)
                .IsRequired();

            // Define constraints for the ApplicationUserId column
            entity
                .Property(t => t.ApplicationUserId)
                .IsRequired();

            // Define relation between the Ticket and CinemaMovie entities
            entity
                .HasOne(t => t.CinemaMovie)
                .WithMany(cm => cm.Tickets)
                .HasForeignKey(t => t.CinemaMovieId)
                .OnDelete(DeleteBehavior.NoAction);

            // Define relation between the Ticket and ApplicationUser entities
            entity
                .HasOne(t => t.ApplicationUser)
                .WithMany(au => au.Ticket)
                .HasForeignKey(t => t.ApplicationUserId)
                .OnDelete(DeleteBehavior.NoAction);

            
        }
    }
}