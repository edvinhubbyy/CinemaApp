using CinemaApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Models
{
    public class CinemaMovie
    {
        [Key]
        public Guid Id { get; set; }

        public Guid MovieId { get; set; }

        public Movie Movie { get; set; } = null!;

        public Guid CinemaId { get; set; }

        public Cinema Cinema { get; set; } = null!;

        public int AvailableTickets { get; set; }

        public bool IsDeleted { get; set; }

        [Unicode(false)]
        [MaxLength(5)]
        public string Showtimes { get; set; } = "00000";

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    }
}

//•	MovieId – Guid, Foreign Key (References Movie) 
//•	Movie – Movie (Navigation Property) 
//•	CinemaId – Guid, Foreign Key (References Cinema) 
//•	Cinema –  Cinema (Navigation Property) 
//•	AvailableTickets – int, Tracks remaining tickets for the movie in a specific cinema 
//•	IsDeleted – bool
//•	Showtimes – string (formatted as "00000", stored as varchar(5))
