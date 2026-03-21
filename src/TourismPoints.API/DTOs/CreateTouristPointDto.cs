using System.ComponentModel.DataAnnotations;

namespace TourismPoints.API.DTOs;

public class CreateTouristPointDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public string Location { get; set; } = string.Empty;
    
    [Required]
    public string City { get; set; } = string.Empty;
    
    [Required]
    [StringLength(2, MinimumLength = 2)]
    public string State { get; set; } = string.Empty;
}