namespace Matrix.Api.Responses;

public record BikeResponse
{
    public Guid BikeId { get; set; }
    
    public string Email { get; set; } = null!;

    public string? Make { get; set; }

    public string? Model { get; set; }
    
    public DateTime? Year { get; set; }
}