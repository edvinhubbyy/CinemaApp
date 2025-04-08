using CinemaApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Models
{
    public class CinemaMovie
    {
        public Guid Id { get; set; }

        public Guid MovieId { get; set; }

        public Movie Movie { get; set; } = null!;

        public Guid CinemaId { get; set; }

        public Cinema Cinema { get; set; } = null!;

        public int AvailableTickets { get; set; }

        public bool IsDeleted { get; set; }
    
        public string? Showtimes { get; set; } = "00000";

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    }
}