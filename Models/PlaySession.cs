namespace GameTracker.Models;

public class PlaySession
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int HoursPlayed { get; set; }

    public int GameId { get; set; }
    public Game Game { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
