using System.ComponentModel.DataAnnotations;

namespace GameTracker.Models;

public class Studio
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public ICollection<Game> Games { get; set; } = new List<Game>();
}
