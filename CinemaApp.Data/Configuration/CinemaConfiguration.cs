﻿using CinemaApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Configuration
{
    internal class CinemaConfiguration : IEntityTypeConfiguration<Cinema>
    {
        public void Configure(EntityTypeBuilder<Cinema> entity)
        {

            entity
                .HasKey(c => c.Id);

            entity
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(256);

            entity
                .Property(c => c.Location)
                .IsRequired()
                .HasMaxLength(256);

            entity
                .Property(c => c.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            entity
                .HasQueryFilter(c => c.IsDeleted == false);

            entity
                .HasData(this.SeedCinemas());

        }

        private IEnumerable<Cinema> SeedCinemas()
        {
            IEnumerable<Cinema> cinemas = new List<Cinema>()
            {
                new Cinema()
                {
                    Id = Guid.NewGuid(),
                    Name = "Cinema city",
                    Location = "Sofia"
                },
                new Cinema()
                {
                    Id = Guid.NewGuid(),
                    Name = "Cinema city",
                    Location = "Plovdiv"
                },
                new Cinema()
                {
                    Id = Guid.NewGuid(),
                    Name = "Cinemax",
                    Location = "Varna"
                }
            };

            return cinemas;
        }

    }
}
