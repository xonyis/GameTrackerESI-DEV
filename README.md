# Game Tracker

Mini-application pour recenser les jeux vidéo, studios, catégories et sessions de jeu.

## Prérequis

- [.NET SDK](https://dotnet.microsoft.com/download) (8 ou 10)

## Lancement

**Application console :**
```bash
dotnet run
```

**Interface web :**
```bash
dotnet run --project GameTracker.Web
```
→ http://localhost:5200

**Créer la solution :**
```bash
dotnet new sln -n GameTracker -o .
dotnet sln add GameTracker.csproj
dotnet sln add GameTracker.Tests/GameTracker.Tests.csproj
dotnet sln add GameTracker.Web/GameTracker.Web.csproj
```

## Tests

```bash
dotnet test GameTracker.Tests/GameTracker.Tests.csproj
```

## Structure

| Dossier         | Rôle                            |
|-----------------|----------------------------------|
| `Models/`       | Entités (User, Game, Category…) |
| `Repositories/` | Couche d'accès aux données      |
| `Data/`         | DbContext, migrations, seeding |
| `GameTracker.Web/` | Interface Razor Pages        |
| `GameTracker.Tests/` | Tests d'intégration       |
