namespace Matrix.Api.Requests;

public record UpdateBikeRequest
{
    public string? Make { get; set; }
    public string? Model { get; set; }
    public DateTime? Year { get; set; }
}