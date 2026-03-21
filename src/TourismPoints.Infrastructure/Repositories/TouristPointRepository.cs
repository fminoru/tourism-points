using Microsoft.EntityFrameworkCore;
using TourismPoints.Domain.Entities;
using TourismPoints.Infrastructure.Context;

namespace TourismPoints.Infrastructure.Repositories;

public class TouristPointRepository : ITouristPointRepository
{
    private readonly TourismDbContext _context;

    public TouristPointRepository(TourismDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<TouristPoint> Items, int TotalCount)> GetAllAsync(
        int page, int pageSize, string? search)
    {
        var query = _context.TouristPoints.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(tp => 
                tp.Name.ToLower().Contains(searchLower) || 
                tp.Description.ToLower().Contains(searchLower) || 
                tp.Location.ToLower().Contains(searchLower));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(tp => tp.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<TouristPoint?> GetByIdAsync(int id)
    {
        return await _context.TouristPoints.FindAsync(id);
    }

    public async Task<TouristPoint> CreateAsync(TouristPoint touristPoint)
    {
        touristPoint.CreatedAt = DateTime.UtcNow;
        _context.TouristPoints.Add(touristPoint);
        await _context.SaveChangesAsync();
        return touristPoint;
    }

    public async Task<TouristPoint> UpdateAsync(TouristPoint touristPoint)
    {
        _context.TouristPoints.Update(touristPoint);
        await _context.SaveChangesAsync();
        return touristPoint;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.TouristPoints.FindAsync(id);
        if (entity == null) return false;

        _context.TouristPoints.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
