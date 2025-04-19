using CinemaApp.Data.Dtos;
using CinemaApp.Data.Models;
using CinemaApp.Data.Utilities.Interfaces;
using CinemaApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using static CinemaApp.Common.OutputMessages.ErrorMessages;

namespace CinemaApp.Data.Utilities
{
    public class DataProcessor : IDbSeeder
    {

        private readonly CinemaDbContext dbContext;

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;

        private readonly IValidator entityValidator;
        private readonly IXmlHelper xmlHelper;
        private readonly ILogger<DataProcessor> logger;

        public DataProcessor(CinemaDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            IValidator entityValidator,
            IXmlHelper xmlHelper,
            ILogger<DataProcessor> logger)
        {
            this.dbContext = dbContext;

            this.userManager = userManager;
            this.roleManager = roleManager;

            this.entityValidator = entityValidator;
            this.xmlHelper = xmlHelper;
            this.logger = logger;
        }
       
        public async Task SeedData()
        {
            await this.SeedRoles();
            await this.SeedUsers();


            // TODO: Implement mechanism for detecting seeded data!
            //await this.ImportMoviesFromJson();
            //await this.ImportCinemaMoviesFromJson();
            //await this.ImportTicketFromXml();
            //await this.ImportWatchlistFromXml();

        }



        private async Task ImportMoviesFromJson()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Files", "movies.json");
            string moviesStr = await File.ReadAllTextAsync(path);
            var movies = JsonSerializer.Deserialize<List<Movie>>(moviesStr);

            if (movies != null && movies.Count > 0)
            {
                List<Guid> moviesIds = movies
                    .Select(m => m.Id)
                    .ToList();
                if (await this.dbContext.Movies.AnyAsync(m => moviesIds.Contains(m.Id)) == false)
                {
                    await this.dbContext.Movies.AddRangeAsync(movies);
                    await this.dbContext.SaveChangesAsync();
                }
            }
        }

        private async Task ImportCinemaMoviesFromJson()
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

                            // Log the message
                            this.logger.LogWarning(this.BuildEntityValidatorWarningMessage(nameof(CinemaMovie)));

                            // Skip the current DTO instance
                            continue;
                        }

                        string[] cinemaInfo = cinemaMobieDto
                            .Cinema
                            .Split(" - ", StringSplitOptions.RemoveEmptyEntries);

                        string cinemaName = cinemaInfo[0];
                        string? cinemaLocation = cinemaInfo.Length > 1 ?
                            cinemaInfo[1] : null;


                        IQueryable<Cinema> cinemaQuery = this.dbContext
                            .Cinemas
                            .Where(c => c.Name == cinemaName);

                        if (cinemaLocation != null)
                        {
                            cinemaQuery = cinemaQuery
                                .Where(c => c.Location == cinemaLocation);
                        }

                        Cinema? cinema = await cinemaQuery
                            .FirstOrDefaultAsync();


                        Movie? movie = await this.dbContext
                            .Movies
                            .FirstOrDefaultAsync(m => m.Title == cinemaMobieDto.Movie);

                        if (cinema == null || movie == null)
                        {
                            // Non existing movie or cinema => cannot import MovieCinema DTO
                            string logMessage = string.Format(EntityImportError, nameof(Ticket)) +
                                ReferencedEntityMissing;

                            this.logger.LogWarning(logMessage);

                            continue;
                        }

                        CinemaMovie? existingProjection = await this.dbContext
                            .CinemasMovies
                            .FirstOrDefaultAsync(cm => cm.CinemaId == cinema.Id &&
                                                    cm.MovieId == movie.Id);

                        if (existingProjection != null &&
                            existingProjection.Showtimes == cinemaMobieDto.Showtimes)
                        {
                            this.logger.LogWarning(EntityInstanceAlreadyExist);

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
                    await this.dbContext.CinemasMovies.AddRangeAsync(validCinemaMovie);
                    await this.dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                throw;
            }

        }

        private async Task ImportTicketFromXml()
        {
            const string rootName = "Tickets";

            string path = Path.Combine(AppContext.BaseDirectory, "Files", "tickets.xml");
            string ticketStr = await File.ReadAllTextAsync(path);

            try
            {
                TicketDto[]? ticketDtos = this.xmlHelper
                    .Deserialize<TicketDto[]>(ticketStr, rootName);

                if (ticketDtos != null && ticketDtos.Length > 0)
                {
                    ICollection<Ticket> validTickets = new List<Ticket>();

                    foreach (TicketDto ticketDto in ticketDtos)
                    {
                        if (!this.entityValidator.IsValid(ticketDto))
                        {
                            this.logger
                                .LogWarning(this.BuildEntityValidatorWarningMessage(nameof(Ticket)));

                            continue;
                        }

                        bool isPriceValid = decimal.TryParse(ticketDto.Price, out decimal ticketPrice);
                        bool isMovieIdValid = Guid.TryParse(ticketDto.MovieId, out Guid ticketMovieId);
                        bool isCinemaIdValid = Guid.TryParse(ticketDto.CinemaId, out Guid ticketCinemaId);
                        bool isUserIdValid = Guid.TryParse(ticketDto.UserId, out Guid ticketUserId);

                        if ((!isPriceValid) || (!isMovieIdValid) || (!isCinemaIdValid) || (!isUserIdValid))
                        {
                            string logMessage = string.Format(EntityImportError, nameof(Ticket)) + EntityDataParseError;
                            this.logger.LogWarning(logMessage);
                            continue;
                        }

                        // Try to find the matching CinemaMovie
                        CinemaMovie? ticketCinemaMovie = await dbContext
                            .CinemasMovies
                            .SingleOrDefaultAsync(cm => cm.CinemaId == ticketCinemaId &&
                                                        cm.MovieId == ticketMovieId);

                        // If not found, create it
                        if (ticketCinemaMovie == null)
                        {
                            ticketCinemaMovie = new CinemaMovie
                            {
                                Id = Guid.NewGuid(),
                                CinemaId = ticketCinemaId,
                                MovieId = ticketMovieId,
                                AvailableTickets = 100,
                                IsDeleted = false,
                                Showtimes = "00000"
                            };

                            dbContext.CinemasMovies.Add(ticketCinemaMovie);
                            await dbContext.SaveChangesAsync(); // Needed to generate the ID
                        }

                        // Validate user
                        ApplicationUser? ticketUser = await dbContext
                            .Users
                            .SingleOrDefaultAsync(u => u.Id == ticketUserId);

                        if (ticketUser == null)
                        {
                            string logMessage = string.Format(EntityImportError, nameof(Ticket)) +
                                                ReferencedEntityMissing;
                            this.logger.LogWarning(logMessage);
                            continue;
                        }

                        // Create Ticket
                        Ticket newTicket = new Ticket
                        {
                            Price = ticketPrice,
                            ApplicationUserId = ticketUserId,
                            CinemaMovieId = ticketCinemaMovie.Id
                        };

                        validTickets.Add(newTicket);
                    }

                    await this.dbContext.Tickets.AddRangeAsync(validTickets);
                    await this.dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                throw;
            }
        }




        private async Task SeedRoles()
        {
            string[] roles = { "Admin", "Manager", "User" };

            foreach (string role in roles)
            {
                bool roleExists = await this.roleManager
                    .RoleExistsAsync(role);
                if (!roleExists)
                {
                    IdentityResult result = await this.roleManager
                        .CreateAsync(new IdentityRole<Guid>(role));
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Format(FailedToCreateRole, role));
                    }
                }
            }
        }

        private async Task SeedUsers()
        {
            await this.SeedUser("admin@example.com", "Admin@123", "Admin");
            await this.SeedUser("appManager@example.com", "123asd", "Manager");
            await this.SeedUser("appUser@example.com", "123asd", "User");
        }

        private async Task SeedUser(string email, string password, string role)
        {
            ApplicationUser? user = await this.userManager
                .FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email
                };

                IdentityResult createUserResult = await this.userManager
                    .CreateAsync(user, password);
                if (!createUserResult.Succeeded)
                {
                    throw new Exception(string.Format(FailedToCreateUser, email));
                }
            }

            bool isInRole = await this.userManager
                .IsInRoleAsync(user, role);
            if (!isInRole)
            {
                IdentityResult addRoleResult = await this.userManager
                    .AddToRoleAsync(user, role);
                if (!addRoleResult.Succeeded)
                {
                    throw new Exception(string.Format(FailedToAssignUserToRole, role, email));
                }
            }
        }

        private async Task ImportWatchlistFromXml()
        {
            const string watchlistRootName = "Users";

            string path = Path.Combine(AppContext.BaseDirectory, "Files", "watchlists.xml");
            string watchlistsStr = await File.ReadAllTextAsync(path);

            try
            {
                UserWatchlistDto[]? watchlistsDtos = xmlHelper
                    .Deserialize<UserWatchlistDto[]>(watchlistsStr, watchlistRootName);

                if (watchlistsDtos != null && watchlistsDtos.Length > 0)
                {
                    ICollection<ApplicationUserMovie> validWatchlists = new List<ApplicationUserMovie>();
                    foreach (UserWatchlistDto watchlistDto in watchlistsDtos)
                    {
                        if (!this.entityValidator.IsValid(watchlistDto))
                        {
                            // Log the message
                            this.logger.LogWarning(this.BuildEntityValidatorWarningMessage(nameof(ApplicationUserMovie)));

                            // Skip if not valid
                            continue;
                        }

                        ApplicationUser? user = await this.userManager
                            .FindByNameAsync(watchlistDto.Username);
                        if (user == null)
                        {
                            // Log the message
                            this.logger.LogWarning(ReferencedEntityMissing);

                            // Skip if not valid
                            continue;
                        }

                        foreach (UserWatchlistMovieDto movieDto in watchlistDto.Movies)
                        {
                            if (!this.entityValidator.IsValid(movieDto))
                            {
                                // Log the message
                                this.logger.LogWarning(this.BuildEntityValidatorWarningMessage(nameof(ApplicationUserMovie)));

                                // Skip if not valid
                                continue;
                            }

                            Movie? movie = await this.dbContext
                                .Movies
                                .FirstOrDefaultAsync(m => m.Title == movieDto.Title);
                            if (movie == null)
                            {
                                this.logger.LogWarning(ReferencedEntityMissing);

                                continue;
                            }

                            ApplicationUserMovie userMovie = new ApplicationUserMovie()
                            {
                                ApplicationUser = user,
                                Movie = movie
                            };
                            validWatchlists.Add(userMovie);
                        }

                    }
                    await dbContext.ApplicationUserMovies.AddRangeAsync(validWatchlists);
                    await dbContext.SaveChangesAsync();

                }
            }
            catch (Exception e)
            {
                this.logger.LogError(e.Message);
                throw;
            }

        }



        private string BuildEntityValidatorWarningMessage(string entityName)
        {
            // Prepare log message with error messages from validation
            StringBuilder logMessage = new StringBuilder();
            logMessage.Append(string.Format(EntityImportError, entityName))
            .AppendLine(string.Join(Environment.NewLine, this.entityValidator.ErrorMessages));

            // Log the message
            return logMessage.ToString().TrimEnd();
        }


    }
}
