using Microsoft.EntityFrameworkCore;
using Movies.API.Data;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MoviesAPIContext>(options =>
    options.UseInMemoryDatabase("Movies"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:5005";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

SeedDatabase(app);

void SeedDatabase(IHost app)
{
    using (var scope = app.Services.CreateScope())
    {
        var movieContext = scope.ServiceProvider.GetRequiredService<MoviesAPIContext>();
        MoviesContextSeed.SeedAsync(movieContext);
    }
}

app.Run();
