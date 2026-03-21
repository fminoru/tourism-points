using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TourismPoints.API.Controllers;
using TourismPoints.API.DTOs;
using TourismPoints.Domain.Entities;
using TourismPoints.Infrastructure.Repositories;

namespace TourismPoints.Tests.Controllers;

public class TouristPointsControllerTests
{
    private readonly Mock<ITouristPointRepository> _repository = new();
    private readonly Mock<ILogger<TouristPointsController>> _logger = new();

    [Fact]
    public async Task GetAll_ReturnsOkWithPagedPayload()
    {
        var items = new List<TouristPoint>
        {
            new()
            {
                Id = 1,
                Name = "Cristo Redentor",
                Description = "Cartao postal do Rio",
                Location = "Parque Nacional da Tijuca",
                City = "Rio de Janeiro",
                State = "RJ",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        };

        _repository
            .Setup(repository => repository.GetAllAsync(2, 5, "rio"))
            .ReturnsAsync((items.AsEnumerable(), 8));

        var controller = CreateController();

        var result = await controller.GetAll(2, 5, "rio");

        var ok = Assert.IsType<OkObjectResult>(result);
        var payload = ok.Value;
        Assert.NotNull(payload);

        Assert.Equal(2, ReadProperty<int>(payload, "Page"));
        Assert.Equal(5, ReadProperty<int>(payload, "PageSize"));
        Assert.Equal(8, ReadProperty<int>(payload, "TotalCount"));
        Assert.Equal(2, ReadProperty<int>(payload, "TotalPages"));

        var returnedItems = Assert.IsAssignableFrom<IEnumerable<TouristPoint>>(
            ReadProperty<object>(payload, "Items"));
        Assert.Single(returnedItems);
    }

    [Fact]
    public async Task GetById_WhenRepositoryReturnsNull_ReturnsNotFound()
    {
        _repository
            .Setup(repository => repository.GetByIdAsync(42))
            .ReturnsAsync((TouristPoint?)null);

        var controller = CreateController();

        var result = await controller.GetById(42);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_WhenModelStateIsInvalid_ReturnsBadRequest()
    {
        var controller = CreateController();
        controller.ModelState.AddModelError("Name", "Required");

        var result = await controller.Create(BuildDto());

        Assert.IsType<BadRequestObjectResult>(result);
        _repository.Verify(repository => repository.CreateAsync(It.IsAny<TouristPoint>()), Times.Never);
    }

    [Fact]
    public async Task Create_WhenDtoIsValid_ReturnsCreatedAtAction()
    {
        TouristPoint? createdEntity = null;

        _repository
            .Setup(repository => repository.CreateAsync(It.IsAny<TouristPoint>()))
            .Callback<TouristPoint>(entity => createdEntity = entity)
            .ReturnsAsync((TouristPoint entity) =>
            {
                entity.Id = 9;
                entity.CreatedAt = new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc);
                return entity;
            });

        var controller = CreateController();
        var dto = BuildDto();

        var result = await controller.Create(dto);

        var created = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(TouristPointsController.GetById), created.ActionName);

        var payload = Assert.IsType<TouristPoint>(created.Value);
        Assert.Equal(9, payload.Id);
        Assert.Equal(dto.Name, payload.Name);
        Assert.NotNull(createdEntity);
        Assert.Equal(dto.Location, createdEntity!.Location);
    }

    [Fact]
    public async Task Delete_WhenRepositoryReturnsFalse_ReturnsNotFound()
    {
        _repository
            .Setup(repository => repository.DeleteAsync(15))
            .ReturnsAsync(false);

        var controller = CreateController();

        var result = await controller.Delete(15);

        Assert.IsType<NotFoundResult>(result);
    }

    private TouristPointsController CreateController()
    {
        return new TouristPointsController(_repository.Object, _logger.Object);
    }

    private static CreateTouristPointDto BuildDto()
    {
        return new CreateTouristPointDto
        {
            Name = "Pelourinho",
            Description = "Centro historico de Salvador",
            Location = "Centro",
            City = "Salvador",
            State = "BA"
        };
    }

    private static T ReadProperty<T>(object value, string propertyName)
    {
        var property = value.GetType().GetProperty(propertyName);
        Assert.NotNull(property);
        var propertyValue = property!.GetValue(value);
        Assert.NotNull(propertyValue);
        return (T)propertyValue;
    }
}
