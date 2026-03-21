using Microsoft.EntityFrameworkCore;
using TourismPoints.Infrastructure.Context;
using TourismPoints.Infrastructure.Data;
using TourismPoints.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure SQLite
builder.Services.AddDbContext<TourismDbContext>(options =>
    options.UseSqlite("Data Source=tourism.db"));

// Register Repository
builder.Services.AddScoped<ITouristPointRepository, TouristPointRepository>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:5175")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReact");
app.UseAuthorization();
app.MapControllers();

// Root URL: API has no default page unless we map it (browsers hitting / alone would 404)
app.MapGet("/", () => Results.Content(
    """
    <!DOCTYPE html>
    <html lang="pt-BR">
    <head>
      <meta charset="utf-8" />
      <meta name="viewport" content="width=device-width, initial-scale=1" />
      <title>Tourism Points API</title>
    </head>
    <body style="font-family: system-ui, sans-serif; max-width: 40rem; margin: 2rem auto; padding: 0 1rem;">
      <h1>Tourism Points API</h1>
      <p>Este endereço é só o <strong>backend</strong>. A interface web roda em outra porta.</p>
      <ul>
        <li><a href="/swagger">Swagger UI</a> — documentação e testes da API</li>
        <li><a href="http://localhost:5175">Aplicação React</a> — inicie com <code>npm run dev</code> em <code>TourismPoints.Client</code> (porta <strong>5175</strong>)</li>
      </ul>
      <p>Exemplo de API: <a href="/api/touristpoints">GET /api/touristpoints</a></p>
    </body>
    </html>
    """,
    "text/html; charset=utf-8"));

// Ensure database is created and seed sample data when empty
await using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TourismDbContext>();
    await context.Database.EnsureCreatedAsync();
    await TourismDbSeeder.SeedAsync(context);
}

app.Run();