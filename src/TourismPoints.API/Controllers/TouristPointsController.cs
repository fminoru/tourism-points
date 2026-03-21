using Microsoft.AspNetCore.Mvc;
using TourismPoints.API.DTOs;
using TourismPoints.Domain.Entities;
using TourismPoints.Infrastructure.Repositories;

namespace TourismPoints.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TouristPointsController : ControllerBase
{
    private readonly ITouristPointRepository _repository;
    private readonly ILogger<TouristPointsController> _logger;

    public TouristPointsController(
        ITouristPointRepository repository,
        ILogger<TouristPointsController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
    {
        var (items, totalCount) = await _repository.GetAllAsync(page, pageSize, search);
        var itemList = items.ToList();

        _logger.LogInformation(
            "Returning {ReturnedCount} tourist points out of {TotalCount} for page {Page} with page size {PageSize} and search term {SearchTerm}",
            itemList.Count,
            totalCount,
            page,
            pageSize,
            search ?? string.Empty);

        return Ok(new
        {
            Items = itemList,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item == null)
        {
            _logger.LogWarning("Tourist point {TouristPointId} was not found", id);
            return NotFound();
        }

        _logger.LogInformation("Returning tourist point {TouristPointId}", id);
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTouristPointDto dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Validation failed while creating a tourist point");
            return BadRequest(ModelState);
        }

        var entity = new TouristPoint
        {
            Name = dto.Name,
            Description = dto.Description,
            Location = dto.Location,
            City = dto.City,
            State = dto.State
        };

        var created = await _repository.CreateAsync(entity);
        _logger.LogInformation(
            "Created tourist point {TouristPointId} for {Name} in {City}/{State}",
            created.Id,
            created.Name,
            created.City,
            created.State);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateTouristPointDto dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Validation failed while updating tourist point {TouristPointId}", id);
            return BadRequest(ModelState);
        }

        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            _logger.LogWarning("Cannot update tourist point {TouristPointId} because it was not found", id);
            return NotFound();
        }

        existing.Name = dto.Name;
        existing.Description = dto.Description;
        existing.Location = dto.Location;
        existing.City = dto.City;
        existing.State = dto.State;

        var updated = await _repository.UpdateAsync(existing);
        _logger.LogInformation(
            "Updated tourist point {TouristPointId} for {Name} in {City}/{State}",
            updated.Id,
            updated.Name,
            updated.City,
            updated.State);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _repository.DeleteAsync(id);
        if (!result)
        {
            _logger.LogWarning("Cannot delete tourist point {TouristPointId} because it was not found", id);
            return NotFound();
        }

        _logger.LogInformation("Deleted tourist point {TouristPointId}", id);
        return NoContent();
    }
}
