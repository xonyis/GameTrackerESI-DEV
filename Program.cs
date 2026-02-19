using System;
using GameTracker.Data;
using GameTracker.Models;
using GameTracker.Repositories;
using Microsoft.EntityFrameworkCore;

var context = new GameTrackerDbContext();
context.Database.Migrate();
DbSeeder.Seed(context);

IGameRepository gameRepo = new GameRepository(context);
ICategoryRepository categoryRepo = new CategoryRepository(context);
IUserRepository userRepo = new UserRepository(context);
IPlaySessionRepository playSessionRepo = new PlaySessionRepository(context);
IStudioRepository studioRepo = new StudioRepository(context);

while (true)
{
    Console.WriteLine("\n=== Game Tracker - Workshop noté ===");
    Console.WriteLine("1 - Lister les jeux d'un utilisateur");
    Console.WriteLine("2 - Lister les jeux d'un utilisateur par catégorie");
    Console.WriteLine("3 - Détail d'un jeu");
    Console.WriteLine("4 - Ajouter un jeu");
    Console.WriteLine("5 - Modifier un jeu");
    Console.WriteLine("6 - Supprimer un jeu");
    Console.WriteLine("7 - Créer une session de jeu");
    Console.WriteLine("8 - Ajouter un jeu à un utilisateur");
    Console.WriteLine("9 - Liste de tous les jeux");
    Console.WriteLine("10 - Liste des catégories");
    Console.WriteLine("0 - Quitter");
    Console.Write("\nChoix : ");

    var choix = Console.ReadLine()?.Trim();

    if (choix == "0")
    {
        Console.WriteLine("Au revoir !");
        break;
    }

    try
    {
        switch (choix)
        {
            case "1":
                ListerJeuxUtilisateur(userRepo, gameRepo);
                break;
            case "2":
                ListerJeuxParCategorie(userRepo, categoryRepo, gameRepo);
                break;
            case "3":
                DetailJeu(gameRepo);
                break;
            case "4":
                AjouterJeu(categoryRepo, studioRepo, gameRepo);
                break;
            case "5":
                ModifierJeu(categoryRepo, studioRepo, gameRepo);
                break;
            case "6":
                SupprimerJeu(gameRepo);
                break;
            case "7":
                CreerSessionJeu(userRepo, gameRepo, playSessionRepo);
                break;
            case "8":
                AjouterJeuAUtilisateur(userRepo, gameRepo, playSessionRepo);
                break;
            case "9":
                ListerTousLesJeux(gameRepo);
                break;
            case "10":
                ListerCategories(categoryRepo);
                break;
            default:
                Console.WriteLine("Option invalide.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erreur : {ex.Message}");
    }
}

static void ListerTousLesJeux(IGameRepository gameRepo)
{
    var jeux = gameRepo.GetAll();
    AfficherJeux(jeux);
}

static void ListerCategories(ICategoryRepository categoryRepo)
{
    var categories = categoryRepo.GetAll();
    if (categories.Count == 0) Console.WriteLine("Aucune catégorie.");
    else
    {
        Console.WriteLine("Catégories :");
        foreach (var c in categories)
            Console.WriteLine($"  {c.Id} - {c.Name}");
    }
}

static void ListerJeuxUtilisateur(IUserRepository userRepo, IGameRepository gameRepo)
{
    var users = userRepo.GetAll();
    if (users.Count == 0) { Console.WriteLine("Aucun utilisateur."); return; }
    Console.WriteLine("Utilisateurs :");
    foreach (var u in users) Console.WriteLine($"  {u.Id} - {u.Username}");
    Console.Write("ID utilisateur : ");
    if (int.TryParse(Console.ReadLine(), out int userId))
    {
        var jeux = gameRepo.GetByUserId(userId);
        AfficherJeux(jeux);
    }
}

static void ListerJeuxParCategorie(IUserRepository userRepo, ICategoryRepository categoryRepo, IGameRepository gameRepo)
{
    var users = userRepo.GetAll();
    var categories = categoryRepo.GetAll();
    if (users.Count == 0 || categories.Count == 0) { Console.WriteLine("Données manquantes."); return; }
    Console.WriteLine("Utilisateurs :");
    foreach (var u in users) Console.WriteLine($"  {u.Id} - {u.Username}");
    Console.Write("ID utilisateur : ");
    if (!int.TryParse(Console.ReadLine(), out int userId)) return;
    Console.WriteLine("Catégories :");
    foreach (var c in categories) Console.WriteLine($"  {c.Id} - {c.Name}");
    Console.Write("ID catégorie : ");
    if (int.TryParse(Console.ReadLine(), out int categoryId))
    {
        var jeux = gameRepo.GetByUserIdAndCategory(userId, categoryId);
        AfficherJeux(jeux);
    }
}

static void AfficherJeux(System.Collections.Generic.List<Game> jeux)
{
    if (jeux.Count == 0) Console.WriteLine("Aucun jeu trouvé.");
    else
        foreach (var g in jeux)
            Console.WriteLine($"  {g.Id} - {g.Title} ({g.ReleaseYear}) - {string.Join(", ", g.Categories.Select(c => c.Name))} - Studios: {string.Join(", ", g.Studios.Select(s => s.Name))}");
}

static void DetailJeu(IGameRepository gameRepo)
{
    Console.Write("ID du jeu : ");
    if (int.TryParse(Console.ReadLine(), out int id))
    {
        var jeu = gameRepo.GetByIdWithRelations(id);
        if (jeu == null) Console.WriteLine("Jeu introuvable.");
        else
        {
            Console.WriteLine($"\n{jeu.Title} ({jeu.ReleaseYear})");
            Console.WriteLine($"Catégories : {string.Join(", ", jeu.Categories.Select(c => c.Name))}");
            Console.WriteLine($"Studios : {string.Join(", ", jeu.Studios.Select(s => s.Name))}");
            Console.WriteLine($"Sessions : {jeu.PlaySessions.Count}");
            foreach (var ps in jeu.PlaySessions)
                Console.WriteLine($"  - {ps.User.Username} : {ps.HoursPlayed}h le {ps.Date:dd/MM/yyyy}");
        }
    }
}

static void AjouterJeu(ICategoryRepository categoryRepo, IStudioRepository studioRepo, IGameRepository gameRepo)
{
    Console.Write("Titre : ");
    var titre = Console.ReadLine();

    Console.Write("Année : ");
    if (string.IsNullOrWhiteSpace(titre) || !int.TryParse(Console.ReadLine(), out int annee))
    {
        Console.WriteLine("Saisie invalide."); return;
    }
    var categories = categoryRepo.GetAll();
    foreach (var c in categories) Console.WriteLine($"  {c.Id} - {c.Name}");

    Console.Write("IDs catégories (séparés par des virgules) : ");
    var idsCat = Console.ReadLine()?.Split(',', StringSplitOptions.RemoveEmptyEntries)
        .Select(s => int.TryParse(s.Trim(), out int id) ? id : 0).Where(i => i > 0).ToList() ?? new List<int>();
    
    Console.WriteLine("Studios :");
    var studios = studioRepo.GetAll();
    foreach (var s in studios) Console.WriteLine($"  {s.Id} - {s.Name}");
    Console.Write("IDs studios (séparés par des virgules, vide pour aucun) : ");
    var idsStudio = Console.ReadLine()?.Split(',', StringSplitOptions.RemoveEmptyEntries)
        .Select(s => int.TryParse(s.Trim(), out int id) ? id : 0).Where(i => i > 0).ToList() ?? new List<int>();

    var game = new Game { Title = titre.Trim(), ReleaseYear = annee };
    foreach (var cat in categories.Where(c => idsCat.Contains(c.Id)))
        game.Categories.Add(cat);
    foreach (var stu in studios.Where(s => idsStudio.Contains(s.Id)))
        game.Studios.Add(stu);

    gameRepo.Add(game);
    Console.WriteLine("Jeu ajouté.");
}

static void ModifierJeu(ICategoryRepository categoryRepo, IStudioRepository studioRepo, IGameRepository gameRepo)
{
    Console.Write("ID du jeu à modifier : ");
    if (!int.TryParse(Console.ReadLine(), out int id)) return;

    var game = gameRepo.GetByIdWithRelations(id);

    if (game == null) { Console.WriteLine("Jeu introuvable."); return; }

    Console.Write($"Nouveau titre ({game.Title}) : ");
    var titre = Console.ReadLine();

    if (!string.IsNullOrWhiteSpace(titre)) game.Title = titre.Trim();

    Console.Write($"Nouvelle année ({game.ReleaseYear}) : ");
    if (int.TryParse(Console.ReadLine(), out int annee)) game.ReleaseYear = annee;

    Console.WriteLine("Catégories actuelles : " + string.Join(", ", game.Categories.Select(c => c.Name)));
    var categories = categoryRepo.GetAll();

    foreach (var c in categories) Console.WriteLine($"  {c.Id} - {c.Name}");

    Console.Write("IDs catégories (séparés par des virgules) : ");

    var idsCat = Console.ReadLine()?.Split(',', StringSplitOptions.RemoveEmptyEntries)
        .Select(s => int.TryParse(s.Trim(), out int cid) ? cid : 0).Where(i => i > 0).ToList() ?? new List<int>();
    if (idsCat.Count > 0)
    {
        game.Categories.Clear();
        foreach (var cat in categories.Where(c => idsCat.Contains(c.Id)))
            game.Categories.Add(cat);
    }

    Console.WriteLine("Studios actuels : " + string.Join(", ", game.Studios.Select(s => s.Name)));
    var studios = studioRepo.GetAll();
    
    foreach (var s in studios) Console.WriteLine($"  {s.Id} - {s.Name}");

    Console.Write("IDs studios (séparés par des virgules) : ");
    var idsStudio = Console.ReadLine()?.Split(',', StringSplitOptions.RemoveEmptyEntries)
        .Select(s => int.TryParse(s.Trim(), out int sid) ? sid : 0).Where(i => i > 0).ToList() ?? new List<int>();

    if (idsStudio.Count > 0)
    {
        game.Studios.Clear();
        foreach (var stu in studios.Where(s => idsStudio.Contains(s.Id)))
            game.Studios.Add(stu);
    }
    gameRepo.Update(id, game);

    Console.WriteLine("Jeu modifié.");
}

static void SupprimerJeu(IGameRepository gameRepo)
{
    Console.Write("ID du jeu à supprimer : ");
    if (int.TryParse(Console.ReadLine(), out int id))
    {
        gameRepo.Delete(id);
        Console.WriteLine("Jeu supprimé.");
    }
}

static void CreerSessionJeu(IUserRepository userRepo, IGameRepository gameRepo, IPlaySessionRepository playSessionRepo)
{
    var users = userRepo.GetAll();
    var games = gameRepo.GetAll();

    if (users.Count == 0 || games.Count == 0) { Console.WriteLine("Données manquantes."); return; }
    
    foreach (var u in users) Console.WriteLine($"  {u.Id} - {u.Username}");
    Console.Write("ID utilisateur : ");
    if (!int.TryParse(Console.ReadLine(), out int userId)) return;
    foreach (var g in games) Console.WriteLine($"  {g.Id} - {g.Title}");
    Console.Write("ID jeu : ");
    if (!int.TryParse(Console.ReadLine(), out int gameId)) return;
    Console.Write("Heures jouées : ");
    if (!int.TryParse(Console.ReadLine(), out int heures)) return;
    var user = userRepo.GetById(userId);
    var game = gameRepo.GetByIdWithRelations(gameId);
    if (user == null || game == null) { Console.WriteLine("Utilisateur ou jeu introuvable."); return; }
    playSessionRepo.Add(new PlaySession { UserId = userId, GameId = gameId, Date = DateTime.Today, HoursPlayed = heures });
    Console.WriteLine("Session créée.");
}

static void AjouterJeuAUtilisateur(IUserRepository userRepo, IGameRepository gameRepo, IPlaySessionRepository playSessionRepo)
{
    var users = userRepo.GetAll();
    var games = gameRepo.GetAll();

    if (users.Count == 0 || games.Count == 0) { Console.WriteLine("Données manquantes."); return; }
    
    Console.WriteLine("Utilisateurs :");
    foreach (var u in users) Console.WriteLine($"  {u.Id} - {u.Username}");

    Console.Write("ID utilisateur : ");
    if (!int.TryParse(Console.ReadLine(), out int userId)) return;

    Console.WriteLine("Jeux disponibles :");
    foreach (var g in games) Console.WriteLine($"  {g.Id} - {g.Title}");

    Console.Write("ID jeu à ajouter : ");
    if (!int.TryParse(Console.ReadLine(), out int gameId)) return;

    Console.Write("Heures jouées (0 si aucune) : ");
    if (!int.TryParse(Console.ReadLine(), out int heures)) heures = 0;

    var user = userRepo.GetById(userId);
    var game = gameRepo.GetByIdWithRelations(gameId);

    if (user == null || game == null) { Console.WriteLine("Utilisateur ou jeu introuvable."); return; }

    playSessionRepo.Add(new PlaySession { UserId = userId, GameId = gameId, Date = DateTime.Today, HoursPlayed = heures });
    Console.WriteLine($"Jeu \"{game.Title}\" ajouté à {user.Username}.");
}
