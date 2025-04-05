using CinemaApp.Common.Constants;
using CinemaApp.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Models
{
    public class Ticket
    {
        
        public Guid Id { get; set; }

       
        public decimal Price { get; set; }

        
        public Guid CinemaMovieId { get; set; }

        public CinemaMovie CinemaMovie { get; set; } = null!;

        
        public Guid ApplicationUserId { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }

    }
}

