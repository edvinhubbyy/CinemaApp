using CinemaApp.Data.Dtos;
using CinemaApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CinemaApp.Data.Utilities
{
    public static class DataProcessor
    {

        public static async Task ImportMoviesFromJson(CinemaDbContext context)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Files", "movies.json");
            string moviesStr = await File.ReadAllTextAsync(path);
            var movies = JsonSerializer.Deserialize<List<Movie>>(moviesStr);

            if(movies != null && movies.Count > 0)
            {
                List<Guid> moviesIds = movies
                    .Select(m => m.Id)
                    .ToList();
                if (await context.Movies.AnyAsync(m => moviesIds.Contains(m.Id)) == false)
                {
                    await context.Movies.AddRangeAsync(movies);
                    await context.SaveChangesAsync();
                }

                await context.Movies.AddRangeAsync(movies);
            }

            await context.Movies.AddRangeAsync(movies);
        }

        public static async Task ImportCinemaMoviesFromJson(CinemaDbContext context)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Files", "cinemasMovies.json");
            string cinemaMoviesStr = await File.ReadAllTextAsync(path);
            var cinemaMovies = JsonSerializer.Deserialize<List<CinemaMovieDto>>(cinemaMoviesStr);

            // Extract movie titles
            var movieTitles = cinemaMovies.Select(m => m.Movie).Distinct().ToArray();

            // Fetch existing movies from DB by title
            var existingMovies = await context
                .Movies
                .Where(m => movieTitles.Contains(m.Title))
                .ToDictionaryAsync(m => m.Title);

            // Add missing movies with new IDs
            foreach (var title in movieTitles)
            {
                if (!existingMovies.ContainsKey(title))
                {
                    var newMovie = new Movie
                    {
                        Id = Guid.NewGuid(),
                        Title = title
                    };

                    existingMovies[title] = newMovie;
                    await context.Movies.AddAsync(newMovie);
                }
            }

            // Handle unique cinemas
            var cinemaGroups = cinemaMovies
                .Select(cm => new
                {
                    Key = cm.Cinema,
                    Name = cm.Cinema.Split('-', StringSplitOptions.RemoveEmptyEntries).First(),
                    Location = cm.Cinema.Contains('-')
                        ? cm.Cinema.Split('-', StringSplitOptions.RemoveEmptyEntries).Last()
                        : string.Empty
                })
                .GroupBy(c => c.Key);

            Dictionary<string, Cinema> cinemas = new();

            foreach (var group in cinemaGroups)
            {
                var cinema = new Cinema
                {
                    Id = Guid.NewGuid(),
                    Name = group.First().Name,
                    Location = group.First().Location
                };
                cinemas[group.Key] = cinema;
            }

            // Create CinemaMovie entries
            List<CinemaMovie> cinemaMovieEntities = new();

            foreach (var cm in cinemaMovies)
            {
                var cinemaMovie = new CinemaMovie
                {
                    Id = Guid.NewGuid(),
                    AvailableTickets = cm.AvailableTickets,
                    Cinema = cinemas[cm.Cinema],
                    Movie = existingMovies[cm.Movie],
                    Showtimes = cm.Showtimes
                };

                cinemaMovieEntities.Add(cinemaMovie);
            }

            await context.Cinemas.AddRangeAsync(cinemas.Values);
            await context.CinemasMovies.AddRangeAsync(cinemaMovieEntities);
            await context.SaveChangesAsync();
        }

        public static async Task ImportTicketFromXml(CinemaDbContext context)
        {
            throw new NotImplementedException();
        }

    }
}
