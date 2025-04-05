namespace CinemaApp.Data.Dtos
{
    public class CinemaMovieDto
    {

        public string Movie { get; set; }

        public string Cinema { get; set; }

        public int AvailableTickets { get; set; }

        public bool IsDeleted { get; set; }

        public string Showtimes { get; set; }

    }
}
