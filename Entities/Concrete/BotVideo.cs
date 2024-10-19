using Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete;

public class BotVideo : IEntity
{
    [Key]
    public int Id { get; set; }

    [MaxLength(500)]
    public string FileId { get; set; } = null!;

    [MaxLength(1000)]
    public string FileUniqueId { get; set; } = null!;

    [ForeignKey("Bot")]
    public int BotId { get; set; }
    public Bot Bot { get; set; } = null!;

    [ForeignKey("Video")]
    public int VideoId { get; set; }
    public Video Video { get; set; } = null!;
}
