using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Models
{
    public class Movie
    {

        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string Genre { get; set; } = null!;

        public DateTime ReleaseDate { get; set; }

        public string Director { get; set; } = null!;

        public int Duration { get; set; }

        public string Description { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<CinemaMovie> MovieCinemas { get; set; }
            = new HashSet<CinemaMovie>();

        public ICollection<ApplicationUserMovie> MovieApplicationUsers { get; set; }
            = new HashSet<ApplicationUserMovie>();


    }
}
