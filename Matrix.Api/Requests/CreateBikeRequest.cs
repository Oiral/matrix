using System.ComponentModel.DataAnnotations;

namespace Matrix.Api.Requests;

public record CreateBikeRequest
{
    [Required]
    public string Email { get; set; } = null!;
    public string? Make { get; set; }
    public string? Model { get; set; }
    public DateTime? Year { get; set; }
}