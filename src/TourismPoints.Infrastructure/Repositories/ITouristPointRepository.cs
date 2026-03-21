using TourismPoints.Domain.Entities;

namespace TourismPoints.Infrastructure.Repositories;

public interface ITouristPointRepository
{
    Task<(IEnumerable<TouristPoint> Items, int TotalCount)> GetAllAsync(int page, int pageSize, string? search);
    Task<TouristPoint?> GetByIdAsync(int id);
    Task<TouristPoint> CreateAsync(TouristPoint touristPoint);
    Task<TouristPoint> UpdateAsync(TouristPoint touristPoint);
    Task<bool> DeleteAsync(int id);
}