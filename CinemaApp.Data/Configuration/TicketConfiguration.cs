using CinemaApp.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaApp.Data.Configuration
{
    internal class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> entity)
        {

            entity
                .HasKey(t => t.Id);

            entity
                .Property(t => t.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");


            entity
                .Property(t => t.ApplicationUserId)
                .IsRequired();

            entity
                .HasOne(t => t.CinemaMovie)
                .WithMany(cm => cm.Tickets)
                .HasForeignKey(t => t.CinemaMovieId)
                .OnDelete(DeleteBehavior.NoAction);

            entity
                .HasOne(t => t.ApplicationUser)
                .WithMany(cm => cm.Ticket)
                .HasForeignKey(t => t.ApplicationUserId)
                .OnDelete(DeleteBehavior.NoAction);
            
        }
    }
}
