using GameTracker.Data;
using GameTracker.Models;
using GameTracker.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GameTracker.Tests;

[DoNotParallelize]
[TestClass]
public sealed class GameRepositoryTests
{
    private SqliteConnection _connection = null!;
    private GameTrackerDbContext _context = null!;
    private IGameRepository _gameRepo = null!;
    private ICategoryRepository _categoryRepo = null!;
    private IUserRepository _userRepo = null!;
    private IStudioRepository _studioRepo = null!;
    private IPlaySessionRepository _playSessionRepo = null!;

    [TestInitialize]
    public void Initialize()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<GameTrackerDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new GameTrackerDbContext(options);
        _context.Database.EnsureCreated();

        _categoryRepo = new CategoryRepository(_context);
        _gameRepo = new GameRepository(_context);
        _userRepo = new UserRepository(_context);
        _studioRepo = new StudioRepository(_context);
        _playSessionRepo = new PlaySessionRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _connection.Dispose();
        _context.Dispose();
    }

    [TestMethod]
    public void CreateGame_ThenGetAll_ShouldHaveOneGame()
    {
        var category = new Category { Name = "RPG" };
        _categoryRepo.Add(category);

        var game = new Game { Title = "The Witcher 3", ReleaseYear = 2015, Categories = [category] };
        _gameRepo.Add(game);

        var games = _gameRepo.GetByUserId(1);
        var allGames = _context.Games.ToList();
        Assert.AreEqual(1, allGames.Count);
        Assert.AreEqual("The Witcher 3", allGames[0].Title);
    }

    [TestMethod]
    public void CreateGameWithStudio_ThenGetByIdWithRelations_ShouldHaveStudio()
    {
        var category = new Category { Name = "RPG" };
        _categoryRepo.Add(category);
        var studio = new Studio { Name = "CD Projekt Red" };
        _studioRepo.Add(studio);

        var studioTracked = _context.Studios.Find(studio.Id);
        var game = new Game { Title = "Cyberpunk 2077", ReleaseYear = 2020, Categories = [category], Studios = studioTracked != null ? [studioTracked] : [] };
        _gameRepo.Add(game);

        var gameWithStudios = _gameRepo.GetByIdWithRelations(game.Id);
        Assert.IsNotNull(gameWithStudios);
        Assert.IsNotNull(gameWithStudios!.Studios);
        Assert.IsTrue(gameWithStudios.Studios.Any(s => s.Name == "CD Projekt Red"));
    }

    [TestMethod]
    public void CreateCategoryAndGame_ThenGetByIdWithRelations_ShouldHaveCategory()
    {
        var category = new Category { Name = "FPS" };
        _categoryRepo.Add(category);
        var game = new Game { Title = "Half-Life 2", ReleaseYear = 2004, Categories = [category] };
        _gameRepo.Add(game);

        var gameWithCategory = _gameRepo.GetByIdWithRelations(game.Id);
        Assert.IsNotNull(gameWithCategory);
        Assert.IsNotNull(gameWithCategory!.Categories);
        Assert.IsTrue(gameWithCategory.Categories.Any(c => c.Name == "FPS"));
    }

    [TestMethod]
    public void CreatePlaySession_ThenGetById_ShouldBeSaved()
    {
        var category = new Category { Name = "RPG" };
        _categoryRepo.Add(category);
        var user = new User { Username = "Alice" };
        _userRepo.Add(user);
        var game = new Game { Title = "Skyrim", ReleaseYear = 2011, Categories = [category] };
        _gameRepo.Add(game);

        var session = new PlaySession { UserId = user.Id, GameId = game.Id, Date = DateTime.Today, HoursPlayed = 10 };
        _playSessionRepo.Add(session);

        var found = _playSessionRepo.GetById(session.Id);
        Assert.IsNotNull(found);
        Assert.AreEqual(10, found!.HoursPlayed);
        Assert.AreEqual(user.Id, found.UserId);
        Assert.AreEqual(game.Id, found.GameId);
    }

    [TestMethod]
    public void GetGameInfos_GetByIdWithRelations_ShouldHaveCategoryAndStudios()
    {
        var category = new Category { Name = "Aventure" };
        _categoryRepo.Add(category);
        var studio = new Studio { Name = "Bethesda" };
        _studioRepo.Add(studio);

        var studioTracked = _context.Studios.Find(studio.Id);
        var game = new Game { Title = "Elder Scrolls", ReleaseYear = 2011, Categories = [category], Studios = studioTracked != null ? [studioTracked] : [] };
        _gameRepo.Add(game);

        var gameFull = _gameRepo.GetByIdWithRelations(game.Id);
        Assert.IsNotNull(gameFull);
        Assert.IsNotNull(gameFull!.Categories);
        Assert.IsTrue(gameFull.Categories.Any(c => c.Name == "Aventure"), "La catégorie doit être chargée");
        Assert.IsNotNull(gameFull.Studios);
        Assert.IsTrue(gameFull.Studios.Any(s => s.Name == "Bethesda"), "Les studios doivent être chargés");
    }

    [TestMethod]
    public void DeleteGame_AfterCreate_ShouldBeRemoved()
    {
        var category = new Category { Name = "Test" };
        _categoryRepo.Add(category);
        var game = new Game { Title = "Jeu à supprimer", ReleaseYear = 2020, Categories = [category] };
        _gameRepo.Add(game);

        _gameRepo.Delete(game.Id);

        var found = _context.Games.Find(game.Id);
        Assert.IsNull(found);
    }
    [TestMethod]
    public void AddGame_WithMultipleCategories_ShouldContainAllCategories()
    {
        // Arrange
        var category1 = new Category { Name = "Aventure" };
        var category2 = new Category { Name = "RPG" };
        _categoryRepo.Add(category1);
        _categoryRepo.Add(category2);
        // _categoryRepo.Add(category3);

        var game = new Game
        {
            Title = "Hollow Knight",
            ReleaseYear = 2017,
            // À commenter pour que le test passe
            Categories = new List<Category> { category1, category2, category3 }
            //  Categories = new List<Category> { category1, category2 }

        };

        _gameRepo.Add(game);

        var found = _gameRepo.GetByIdWithRelations(game.Id);

        Assert.IsNotNull(found);
        Assert.AreEqual(2, found!.Categories.Count);
        Assert.IsTrue(found.Categories.Any(c => c.Name == "Aventure"));
        Assert.IsTrue(found.Categories.Any(c => c.Name == "RPG"));

    }
}
