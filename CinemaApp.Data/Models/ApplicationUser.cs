using CinemaApp.Models;
using Microsoft.AspNetCore.Identity;

namespace CinemaApp.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {

        public ApplicationUser()
        {
            this.Id = Guid.NewGuid();
        }

        public ICollection<ApplicationUserMovie> Watchlist { get; set; }
            = new HashSet<ApplicationUserMovie>();

        public ICollection<Ticket> Ticket { get; set; }
            = new HashSet<Ticket>();

    }
}
