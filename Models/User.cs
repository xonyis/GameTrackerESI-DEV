using System.ComponentModel.DataAnnotations;

namespace GameTracker.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    public ICollection<PlaySession> PlaySessions { get; set; } = new List<PlaySession>();
}
