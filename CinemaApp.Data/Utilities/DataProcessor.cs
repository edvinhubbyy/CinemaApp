using CinemaApp.Data.Dtos;
using CinemaApp.Data.Models;
using CinemaApp.Data.Utilities.Interfaces;
using CinemaApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using static CinemaApp.Common.OutputMessages.ErrorMessages;

namespace CinemaApp.Data.Utilities
{
    public class DataProcessor
    {

        private readonly IValidator entityValidator;
        private readonly ILogger<DataProcessor> logger;

        public DataProcessor(IValidator entityValidator, ILogger<DataProcessor> logger)
        {
            this.entityValidator = entityValidator;
            this.logger = logger;
        }

        public async Task ImportMoviesFromJson(CinemaDbContext context)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Files", "movies.json");
            string moviesStr = await File.ReadAllTextAsync(path);
            var movies = JsonSerializer.Deserialize<List<Movie>>(moviesStr);

            if (movies != null && movies.Count > 0)
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

        public async Task ImportCinemaMoviesFromJson(CinemaDbContext context)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Files", "cinemasMovies.json");
            string cinemaMoviesStr = await File.ReadAllTextAsync(path);

            try
            {


                CinemaMovieDto[]? cinemaMovieDtos =
                    JsonSerializer.Deserialize<CinemaMovieDto[]>(cinemaMoviesStr);
                if (cinemaMovieDtos != null! && cinemaMovieDtos.Length > 0)
                {
                    ICollection<CinemaMovie> validCinemaMovie = new List<CinemaMovie>();
                    foreach (CinemaMovieDto cinemaMobieDto in cinemaMovieDtos)
                    {
                        if (!this.entityValidator.IsValid(cinemaMobieDto))
                        {
                            // Prepare log message with error messages from validation
                            StringBuilder logMessage = new StringBuilder();
                            logMessage.Append(string.Format(EntityImportError, nameof(CinemaMovie)))
                            .AppendLine(string.Join(Environment.NewLine, this.entityValidator.ErrorMessages));

                            // Log the message
                            this.logger.LogWarning(logMessage.ToString().TrimEnd());

                            // Skip the current DTO instance
                            continue;
                        }

                        string[] cinemaInfo = cinemaMobieDto
                            .Cinema
                            .Split(" - ", StringSplitOptions.RemoveEmptyEntries);

                        string cinemaName = cinemaInfo[0];
                        string? cinemaLocation = cinemaInfo.Length > 1 ?
                            cinemaInfo[1] : null;


                        IQueryable<Cinema> cinemaQuery = context
                            .Cinemas
                            .Where(c => c.Name == cinemaName);

                        if (cinemaLocation != null)
                        {
                            cinemaQuery = cinemaQuery
                                .Where(c => c.Location == cinemaLocation);
                        }

                        Cinema? cinema = await cinemaQuery
                            .FirstOrDefaultAsync();


                        Movie? movie = await context
                            .Movies
                            .FirstOrDefaultAsync(m => m.Title == cinemaMobieDto.Movie);

                        if (cinema == null || movie == null)
                        {
                            // Non existing movie or cinema => cannot import MovieCinema DTO
                            string logMessage = string.Format(EntityImportError, nameof(CinemaMovie)) +
                                ReferencedEntityMissing;

                            this.logger.LogWarning(logMessage);

                            continue;
                        }

                        CinemaMovie? existingProjection = await context
                            .CinemasMovies
                            .FirstOrDefaultAsync(cm => cm.CinemaId == cinema.Id &&
                                                    cm.MovieId == movie.Id);

                        if (existingProjection != null &&
                            existingProjection.Showtimes == cinemaMobieDto.Showtimes)
                        {
                            this.logger.LogWarning(EntityInstanceAlreadyExists);

                            continue;
                        }

                        CinemaMovie newCinemaMovie = new CinemaMovie()
                        {
                            CinemaId = cinema.Id,
                            MovieId = movie.Id,
                            AvailableTickets = cinemaMobieDto.AvailableTickets,
                            IsDeleted = cinemaMobieDto.IsDeleted,
                            Showtimes = cinemaMobieDto.Showtimes
                        };
                        validCinemaMovie.Add(newCinemaMovie);
                    }
                    await context.CinemasMovies.AddRangeAsync(validCinemaMovie);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                throw;
            }

        }

        public async Task ImportTicketFromXml(CinemaDbContext context)
        {
            throw new NotImplementedException();
        }


        public void SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            string[] roles = { "Admin", "Manager", "User" };

            foreach (var role in roles)
            {
                var roleExists = roleManager.RoleExistsAsync(role).GetAwaiter().GetResult();
                if (!roleExists)
                {
                    var result = roleManager.CreateAsync(new IdentityRole<Guid>(role)).GetAwaiter().GetResult();
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Failed to create role: {role}");
                    }
                }
            }
        }

        public void SeedUsers(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            SeedUser(userManager, "admin@example.com", "Admin@123", "Admin");
            SeedUser(userManager, "appManager@example.com", "123asd", "Manager");
            SeedUser(userManager, "appUser@example.com", "123asd", "User");
        }

        private void SeedUser(UserManager<ApplicationUser> userManager, string email, string password, string role)
        {
            var user = userManager.FindByEmailAsync(email).GetAwaiter().GetResult();
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email
                };
                var createUserResult = userManager.CreateAsync(user, password).GetAwaiter().GetResult();
                if (!createUserResult.Succeeded)
                {
                    throw new Exception($"Failed to create user: {email}");
                }
            }

            var isInRole = userManager.IsInRoleAsync(user, role).GetAwaiter().GetResult();
            if (!isInRole)
            {
                var addRoleResult = userManager.AddToRoleAsync(user, role).GetAwaiter().GetResult();
                if (!addRoleResult.Succeeded)
                {
                    throw new Exception($"Failed to assign {role} role to user: {email}");
                }
            }



        }
    }
}
