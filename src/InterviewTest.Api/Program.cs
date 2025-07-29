using Microsoft.EntityFrameworkCore;
using InterviewTest.Infrastructure.Data;
using InterviewTest.Core.Interfaces;
using InterviewTest.Infrastructure.Repositories;
using InterviewTest.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Entity Framework
var useInMemoryDb = builder.Configuration.GetValue<bool>("UseInMemoryDatabase", true);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (useInMemoryDb)
    {
        // Use In-Memory database for easy setup during interviews
        options.UseInMemoryDatabase("InterviewTestDb");
    }
    else
    {
        // Use SQL Server LocalDB or Docker SQL Server
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (builder.Environment.IsProduction() || builder.Configuration.GetValue<bool>("UseDockerDatabase"))
        {
            connectionString = builder.Configuration.GetConnectionString("DockerConnection");
        }
        options.UseSqlServer(connectionString);
    }
});

// Register repositories and services
builder.Services.AddScoped<IGuestRepository, GuestRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<BookingService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "Interview Test API", 
        Version = "v1",
        Description = "A simple hotel booking API for developer interviews"
    });
});

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
    await DataSeeder.SeedDataAsync(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Interview Test API v1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at root
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
