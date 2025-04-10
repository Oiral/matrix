namespace Matrix.Entities.Models;

public class Bike
{
    public Guid BikeId { get; set; }
    
    public string Email { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime LastUpdatedAt { get; set; }

    public string? Make { get; set; }

    public string? Model { get; set; }
    
    public DateTime? Year { get; set; }
}