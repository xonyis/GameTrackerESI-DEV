using GameTracker.Data;
using GameTracker.Repositories;
using Microsoft.EntityFrameworkCore;

var context = new GameTrackerDbContext();
context.Database.Migrate();
DbSeeder.Seed(context);

IGameRepository gameRepo = new GameRepository(context);
ICategoryRepository categoryRepo = new CategoryRepository(context);
IUserRepository userRepo = new UserRepository(context);
IPlaySessionRepository playSessionRepo = new PlaySessionRepository(context);

var users = userRepo.GetAll();
var userId = users.First().Id;

Console.WriteLine("=== Game Tracker - Workshop noté ===\n");

Console.WriteLine("--- Liste des jeux d'un utilisateur (avec relations) ---");
var gamesForUser = gameRepo.GetByUserId(userId);
foreach (var g in gamesForUser)
{
    Console.WriteLine($"  {g.Title} - {string.Join(", ", g.Categories.Select(c => c.Name))} - Studios: {string.Join(", ", g.Studios.Select(s => s.Name))}");
}

Console.WriteLine("\n--- Liste par catégorie ---");
var categories = categoryRepo.GetAll();
foreach (var cat in categories)
{
    var gamesInCat = gameRepo.GetByUserIdAndCategory(userId, cat.Id);
    if (gamesInCat.Any())
        Console.WriteLine($"  [{cat.Name}] {string.Join(", ", gamesInCat.Select(g => g.Title))}");
}

var firstGame = gamesForUser.First();
Console.WriteLine($"\n--- Détail du jeu {firstGame.Id} ---");
var detail = gameRepo.GetByIdWithRelations(firstGame.Id);
if (detail != null)
{
    Console.WriteLine($"  Titre: {detail.Title} | Année: {detail.ReleaseYear}");
    Console.WriteLine($"  Catégories: {string.Join(", ", detail.Categories.Select(c => c.Name))}");
    Console.WriteLine($"  Studios: {string.Join(", ", detail.Studios.Select(s => s.Name))}");
    Console.WriteLine($"  Sessions: {detail.PlaySessions.Count}");
}

Console.WriteLine("\n--- Insertion d'un nouveau jeu ---");
var newCategory = categoryRepo.GetById(1);
var studio = context.Studios.Find(1);
var newGame = new GameTracker.Models.Game { Title = "Cyberpunk 2077", ReleaseYear = 2020, Categories = newCategory != null ? [newCategory] : [], Studios = studio != null ? [studio] : [] };
gameRepo.Add(newGame);
Console.WriteLine($"  Ajouté: {newGame.Title} (Id={newGame.Id})");

Console.WriteLine("\n--- Modification d'un jeu ---");
var gameToUpdate = gameRepo.GetByIdWithRelations(newGame.Id)!;
gameToUpdate.ReleaseYear = 2023;
gameRepo.Update(newGame.Id, gameToUpdate);
Console.WriteLine($"  Modifié: {newGame.Title} -> Année {newGame.ReleaseYear}");

Console.WriteLine("\n--- Insertion d'un utilisateur ---");
var newUser = new GameTracker.Models.User { Username = "Charlie" };
userRepo.Add(newUser);
Console.WriteLine($"  Ajouté: {newUser.Username} (Id={newUser.Id})");

Console.WriteLine("\n--- Insertion d'une catégorie ---");
var newCat = new GameTracker.Models.Category { Name = "Stratégie" };
categoryRepo.Add(newCat);
Console.WriteLine($"  Ajouté: {newCat.Name} (Id={newCat.Id})");

Console.WriteLine("\n--- Modification d'une catégorie ---");
newCat.Name = "Stratégie tactique";
categoryRepo.Update(newCat.Id, newCat);
Console.WriteLine($"  Modifié: {newCat.Name}");

Console.WriteLine($"\n--- Modification heures jouées (session User={userId}, Game={firstGame.Id}) ---");
var updated = playSessionRepo.UpdateHours(userId, firstGame.Id, 15);
Console.WriteLine(updated ? "  Heures mises à jour: 15" : "  Session non trouvée");

Console.WriteLine("\n--- Suppression du jeu ajouté ---");
gameRepo.Delete(newGame.Id);
Console.WriteLine($"  Supprimé: {newGame.Title}");

Console.WriteLine("\n--- Suppression de la catégorie ajoutée ---");
categoryRepo.Delete(newCat.Id);
Console.WriteLine($"  Supprimé: {newCat.Name}");

Console.WriteLine("\n=== Fin du programme ===");
