using Core.Entities;

namespace Entities.Dtos;

public class PhotoDto : IDto
{
    public string FileId { get; set; } = null!;
    public string FileUniqueId { get; set; } = null!;
    public int Height { get; set; }
    public int Width { get; set; }
    public int FileSize { get; set; }
}
