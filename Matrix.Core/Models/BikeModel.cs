namespace Matrix.Core.Models;

public record BikeModel
{
    public Guid BikeId { get; set; }
    
    public string Email { get; set; } = null!;

    public string? Make { get; set; }

    public string? Model { get; set; }
    
    public DateTime? Year { get; set; }
}