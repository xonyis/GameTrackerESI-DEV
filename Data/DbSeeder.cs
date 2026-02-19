using GameTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace GameTracker.Data;

public static class DbSeeder
{
    public static void Seed(GameTrackerDbContext context)
    {
        if (context.Users.Any())
            return;

        var rpg = new Category { Name = "RPG" };
        var fps = new Category { Name = "FPS" };
        var aventure = new Category { Name = "Aventure" };
        context.Categories.AddRange(rpg, fps, aventure);
        context.SaveChanges();

        var cdpr = new Studio { Name = "CD Projekt Red" };
        var bethesda = new Studio { Name = "Bethesda" };
        var valve = new Studio { Name = "Valve" };
        context.Studios.AddRange(cdpr, bethesda, valve);
        context.SaveChanges();

        var witcher = new Game { Title = "The Witcher 3", ReleaseYear = 2015, Categories = [rpg], Studios = [cdpr] };
        var skyrim = new Game { Title = "The Elder Scrolls V: Skyrim", ReleaseYear = 2011, Categories = [rpg, aventure], Studios = [bethesda] };
        var halfLife = new Game { Title = "Half-Life 2", ReleaseYear = 2004, Categories = [fps], Studios = [valve] };
        context.Games.AddRange(witcher, skyrim, halfLife);
        context.SaveChanges();

        var alice = new User { Username = "Alice" };
        var bob = new User { Username = "Bob" };
        context.Users.AddRange(alice, bob);
        context.SaveChanges();

        context.PlaySessions.AddRange(
            new PlaySession { UserId = alice.Id, GameId = witcher.Id, Date = DateTime.Today.AddDays(-5), HoursPlayed = 12 },
            new PlaySession { UserId = alice.Id, GameId = skyrim.Id, Date = DateTime.Today.AddDays(-2), HoursPlayed = 8 },
            new PlaySession { UserId = bob.Id, GameId = halfLife.Id, Date = DateTime.Today.AddDays(-1), HoursPlayed = 5 }
        );
        context.SaveChanges();
    }
}
