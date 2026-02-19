# Game Tracker - Interface Web

Interface web Razor Pages pour le projet Game Tracker (bonus).

## Lancer l'application

Depuis le dossier `Workshop noté` :

```bash
dotnet run --project GameTracker.Web
```

Puis ouvrir **http://localhost:5001** dans le navigateur.

## Pages disponibles

- **Accueil** : Dashboard avec liens vers les sections
- **Jeux** : Liste des jeux (titre, année, catégorie, studios)
- **Détail jeu** : Fiche complète avec sessions de jeu
- **Catégories** : Liste des catégories
- **Utilisateurs** : Liste des utilisateurs
- **Jeux joués** : Pour chaque utilisateur, les jeux qu'il a joués

## Base de données

Utilise la même base SQLite `gametracker.db` que le programme console. Lancer depuis `Workshop noté` pour que le chemin soit correct.
