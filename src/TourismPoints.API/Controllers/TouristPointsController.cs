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

    public TouristPointsController(ITouristPointRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
    {
        var (items, totalCount) = await _repository.GetAllAsync(page, pageSize, search);
        
        return Ok(new 
        {
            Items = items,
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
            return NotFound();
        
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTouristPointDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = new TouristPoint
        {
            Name = dto.Name,
            Description = dto.Description,
            Location = dto.Location,
            City = dto.City,
            State = dto.State
        };

        var created = await _repository.CreateAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateTouristPointDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            return NotFound();

        existing.Name = dto.Name;
        existing.Description = dto.Description;
        existing.Location = dto.Location;
        existing.City = dto.City;
        existing.State = dto.State;

        var updated = await _repository.UpdateAsync(existing);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _repository.DeleteAsync(id);
        if (!result)
            return NotFound();
        
        return NoContent();
    }
}
