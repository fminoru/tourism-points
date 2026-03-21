using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TourismPoints.Domain.Entities;
using TourismPoints.Infrastructure.Context;
using TourismPoints.Infrastructure.Repositories;

namespace TourismPoints.Tests.Repositories;

public class TouristPointRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_FiltersAndPaginatesByCreatedAtDescending()
    {
        await using var database = await CreateDatabaseAsync();

        database.Context.TouristPoints.AddRange(
            new TouristPoint
            {
                Name = "Praia da Pipa",
                Description = "Praia com falésias",
                Location = "Litoral sul",
                City = "Tibau do Sul",
                State = "RN",
                CreatedAt = new DateTime(2024, 1, 10, 0, 0, 0, DateTimeKind.Utc)
            },
            new TouristPoint
            {
                Name = "Praia do Forte",
                Description = "Praia famosa na Bahia",
                Location = "Mata de São João",
                City = "Salvador",
                State = "BA",
                CreatedAt = new DateTime(2024, 1, 20, 0, 0, 0, DateTimeKind.Utc)
            },
            new TouristPoint
            {
                Name = "Cataratas do Iguaçu",
                Description = "Quedas d'água",
                Location = "Parque Nacional",
                City = "Foz do Iguaçu",
                State = "PR",
                CreatedAt = new DateTime(2024, 1, 30, 0, 0, 0, DateTimeKind.Utc)
            });

        await database.Context.SaveChangesAsync();

        var repository = new TouristPointRepository(database.Context);

        var (items, totalCount) = await repository.GetAllAsync(1, 1, "praia");

        var firstItem = Assert.Single(items);
        Assert.Equal(2, totalCount);
        Assert.Equal("Praia do Forte", firstItem.Name);
    }

    [Fact]
    public async Task CreateAsync_SetsCreatedAtAndPersistsEntity()
    {
        await using var database = await CreateDatabaseAsync();
        var repository = new TouristPointRepository(database.Context);

        var entity = new TouristPoint
        {
            Name = "Lençóis Maranhenses",
            Description = "Parque nacional com lagoas",
            Location = "Barreirinhas",
            City = "Barreirinhas",
            State = "MA"
        };

        var created = await repository.CreateAsync(entity);

        Assert.True(created.Id > 0);
        Assert.True(created.CreatedAt <= DateTime.UtcNow);
        Assert.Equal(1, await database.Context.TouristPoints.CountAsync());
    }

    [Fact]
    public async Task UpdateAsync_PersistsChanges()
    {
        await using var database = await CreateDatabaseAsync();

        var entity = new TouristPoint
        {
            Name = "Marco Zero",
            Description = "Centro do Recife",
            Location = "Recife Antigo",
            City = "Recife",
            State = "PE",
            CreatedAt = new DateTime(2024, 1, 5, 0, 0, 0, DateTimeKind.Utc)
        };

        database.Context.TouristPoints.Add(entity);
        await database.Context.SaveChangesAsync();

        var repository = new TouristPointRepository(database.Context);

        entity.Description = "Praça histórica no Recife Antigo";

        await repository.UpdateAsync(entity);

        var updated = await database.Context.TouristPoints.SingleAsync();
        Assert.Equal("Praça histórica no Recife Antigo", updated.Description);
    }

    [Fact]
    public async Task DeleteAsync_WhenEntityExists_RemovesEntity()
    {
        await using var database = await CreateDatabaseAsync();

        var entity = new TouristPoint
        {
            Name = "Chapada dos Veadeiros",
            Description = "Parque nacional",
            Location = "Alto Paraíso",
            City = "Alto Paraíso de Goiás",
            State = "GO",
            CreatedAt = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc)
        };

        database.Context.TouristPoints.Add(entity);
        await database.Context.SaveChangesAsync();

        var repository = new TouristPointRepository(database.Context);

        var removed = await repository.DeleteAsync(entity.Id);

        Assert.True(removed);
        Assert.Empty(database.Context.TouristPoints);
    }

    private static async Task<TestDatabase> CreateDatabaseAsync()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<TourismDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new TourismDbContext(options);
        await context.Database.EnsureCreatedAsync();

        return new TestDatabase(context, connection);
    }

    private sealed class TestDatabase(
        TourismDbContext context,
        SqliteConnection connection) : IAsyncDisposable
    {
        public TourismDbContext Context { get; } = context;

        public async ValueTask DisposeAsync()
        {
            await Context.DisposeAsync();
            await connection.DisposeAsync();
        }
    }
}
