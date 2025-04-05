using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace CinemaApp.Models
{
    [Comment("Cinema in the system")]
    public class Cinema
    {

        [Key]
        [Comment("Cinema identifier")]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(256)]
        [Comment("Cinema name")]
        public string Name { get; set; } = null!;

        [Required]
        public string Location { get; set; } = null!;

        public bool IsDeleted { get; set; }

        public ICollection<CinemaMovie> CinemaMovies { get; set; }
           = new List<CinemaMovie>();

        public ICollection<Ticket> Tickets { get; set; }
           = new HashSet<Ticket>();

    }
}