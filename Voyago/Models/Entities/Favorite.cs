namespace Voyago.Models.Entities;
public sealed class Favorite { public int Id { get; set; } public string UserId { get; set; } = string.Empty; public int DestinationId { get; set; } public DateTime CreatedAtUtc { get; set; } public ApplicationUser User { get; set; } = null!; public Destination Destination { get; set; } = null!; }
