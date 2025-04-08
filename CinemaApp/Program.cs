using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Data.Utilities;
using CinemaApp.Data.Utilities.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("CinemaDbConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<CinemaDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<IValidator, EntityValidator>();

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<CinemaDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;

    CinemaDbContext dbContext = services.GetRequiredService<CinemaDbContext>();
    IValidator entityValidtor = services.GetRequiredService<IValidator>();
    ILogger<DataProcessor> logger = services.GetRequiredService<ILogger<DataProcessor>>();

    DataProcessor dataProcessor = new DataProcessor(entityValidtor, logger);
    await dataProcessor.ImportCinemaMoviesFromJson(dbContext);
    // Uncomment so u import the shit 

    //await DataProcessor.ImportMoviesFromJson(dbContext);
    //await DataProcessor.ImportCinemaMoviesFromJson(dbContext);
    //await DataProcessor.ImportTicketFromXml(dbContext);
}


app.Run();
