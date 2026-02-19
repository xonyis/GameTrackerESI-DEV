using System.ComponentModel.DataAnnotations;

namespace GameTracker.Models;

public class Game
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public int? ReleaseYear { get; set; }

    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Studio> Studios { get; set; } = new List<Studio>();

    public ICollection<PlaySession> PlaySessions { get; set; } = new List<PlaySession>();
}
