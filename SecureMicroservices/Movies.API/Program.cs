using Microsoft.EntityFrameworkCore;
using Movies.API.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MoviesAPIContext>(options =>
    options.UseInMemoryDatabase(("Movies")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", option => 
    {
        option.Authority = "https://localhost:5005";
        option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters() 
        {
            ValidateAudience = false
        };
    });

var app = builder.Build();
SeedDatabase(app);

void SeedDatabase(WebApplication app)
{
    using(var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var movieContext = services.GetRequiredService<MoviesAPIContext>();
        MoviesContextSeed.SeedAsync(movieContext);
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
