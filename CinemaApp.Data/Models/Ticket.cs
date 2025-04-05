using CinemaApp.Common.Constants;
using CinemaApp.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Models
{
    public class Ticket
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid CinemaMovieId { get; set; }

        public CinemaMovie CinemaMovie { get; set; } = null!;

        [Required]
        public Guid ApplicationUserId { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }

    }
}

