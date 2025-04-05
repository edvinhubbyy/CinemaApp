using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace CinemaApp.Models
{
    [Comment("Cinema in the system")]
    public class Cinema
    {

        [Comment("Cinema identifier")]
        public Guid Id { get; set; }

        
        [Comment("Cinema name")]
        public string Name { get; set; } = null!;

        public string Location { get; set; } = null!;

        public bool IsDeleted { get; set; }

        public ICollection<CinemaMovie> CinemaMovies { get; set; }
           = new List<CinemaMovie>();

    }
}