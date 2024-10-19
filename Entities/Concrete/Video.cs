using Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete;

public class Video : IEntity
{
    [Key]
    public int Id { get; set; }

    public string? Path { get; set; }

    public string MimeType { get; set; } = null!;

    public int Height { get; set; }

    public int Width { get; set; }

    public int Duration { get; set; }

    public int FileSize { get; set; }

    public DateTime CreateDate { get; set; } = DateTime.Now;


    [ForeignKey("User")]
    public int PublisherUserId { get; set; }
    public User PublisherUser { get; set; } = null!;

    [ForeignKey("Photo")]
    public int PhotoId { get; set; }
    public Photo Photo { get; set; } = null!;

    public List<BotVideo> BotVideos { get; set; } = null!;
}
