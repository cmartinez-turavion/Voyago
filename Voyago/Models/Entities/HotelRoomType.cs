namespace Voyago.Models.Entities;
public sealed class HotelRoomType { public int Id { get; set; } public int HotelId { get; set; } public string Name { get; set; } = string.Empty; public decimal Size { get; set; } public string BedType { get; set; } = string.Empty; public int Capacity { get; set; } public decimal PricePerNight { get; set; } public Hotel Hotel { get; set; } = null!; }
