# Évaluation - Workshop Entity Framework Core
- L'évaluation sera essentiellement notée par rapport aux objectifs proposés plus bas.
- **L'implémentation d'Identity pour une bonne gestion des utilisateurs est en bonus.** (Mais je pense qu'il y a déjà assez à faire comme ça... =D)
- Vous pouvez réaliser l'évaluation en groupe de 2 ou en solo (pas de groupe de 3 !)

## Contexte
Vous devez développer une mini-application utile et réaliste permettant à un utilisateur de suivre ce qu’il consomme dans son temps libre.
Deux thèmes possibles (au choix) :
- Manga Tracker, un outil permettant de recenser :
    - les mangas lus
    - leurs auteurs
    - leur catégorie (shonen, manhwa,...)
    - les lectures effectuées par les utilisateurs
- Game Tracker, un outil permettant de recenser :
    - les jeux vidéo joués
    - leur studio ou éditeur
    - leur catégorie (RPG, FPS,...)
    - les sessions de jeu effectuées par les utilisateurs
En soit le fonctionnement est le même dans les deux cas

## Objectifs attendus
Vous devez produire un projet fonctionnel contenant :
- des entités avec relations
- une base SQLite créée via migrations
- des opérations CRUD simples
- une couche Repository
- une requête optimisée avec Include (Eager Loading)
- une requête en lecture seule avec AsNoTracking
- une suite de tests d’intégration SQLite in-memory
- tout peut être géré par un programme console, mais vous pouvez également vous amuser à faire une interface graphique

## P1 : Création du modèle de données
Vous devez créer les entités suivantes.

## Modèle commun (Option Manga ou Jeux)
### User
- Id
- Username (Required, MaxLength 100)
- Relation avec ReadingSession / PlaySession (One-to-Many)

### Category
- Id
- Name (Required, MaxLength 100)
- Relation One-to-Many avec Manga ou Game

### Manga ou Game (au choix)
#### Manga
- Id
- Title (Required, MaxLength 200)
- PublishedYear
- Relation Many-to-Many avec Author
- Relation Many-to-One avec Category
- Relation One-to-Many avec ReadingSession

#### Game
- Id
- Title (Required, MaxLength 200)
- ReleaseYear
- Relation Many-to-Many avec Studio
- Relation Many-to-One avec Category
- Relation One-to-Many avec PlaySession

### Author ou Studio (au choix)
#### Author
- Id
- Name (Required, MaxLength 100)
- Relation Many-to-Many avec Manga

#### Studio
- Id
- Name (Required, MaxLength 100)
- Relation Many-to-Many avec Game

### ReadingSession ou PlaySession (au choix)
#### ReadingSession
- Id
- Date
- ChaptersRead (int)
- Relation Many-to-One vers Manga
- Relation Many-to-One vers User

#### PlaySession
- Id
- Date
- HoursPlayed (int)
- Relation Many-to-One vers Game
- Relation Many-to-One vers User

## P2 - DbContext et migrations
A vous de créer le DbContext et appliquer les migrations

## P3 - Données initiales et requêtes demandées 
L’application doit permettre au minimum :

### Initialisation
- Insérer automatiquement quelques catégories (via un seeding, ou via un script à l'initialisation de la BDD)
- Insérer quelques mangas / jeux (avec leurs catégories)
- Insérer deux utilisateurs

### Requêtes demandées
- **ATTENTION : Pensez bien à faire une couche repository propre comme on a appris à le faire !**
- Lister les mangas / jeux d'un utilisateur 
    - Liste full avec les relations
    - Liste par catégorie
- Détail d'un manga / jeu (avec les relations chargées et un affichage complet)
- Insérer un manga ou jeu
- Modifier un manga ou jeu
- Supprimer un manga ou jeu
- Insérer des nouveaux utilisateurs
- Insérer une catégorie
- Modifier une catégorie
- Supprimer une catégorie
- Modifier le nombre d'heures ou le nombre de chapitres lus d'un manga que l'utilisateur a associé à son compte

## P4 - Chargement des relations
Pour afficher correctement les mangas/jeux avec leurs relations, vous devez utiliser Include et donc faire du EagerLoading.

## P5 - Tracking vs AsNoTracking
- Les lectures seront à appliquer en AsNoTracking
- Les modifications seront à appliquer en Tracking (classique)

## P6 - Tests d’intégration SQLite in-memory
### Tests minimus attendus

#### Test 1 - Insertion d’un manga/jeu
- Ajouter un objet via repository
- Vérifier qu’il existe en base

#### Test 2 - Relation Many-to-Many (Author/Studio)
- Ajouter un auteur/studio + un manga/jeu lié
- Vérifier que la relation fonctionne

#### Test 3 - Relation Category → Manga/Game
- Ajouter une catégorie + un manga/jeu associé
- Vérifier la navigation

#### Test 4 - Création d’une session
- Ajouter une session de lecture/jeu
- Vérifier qu’elle est bien enregistrée

### Test 5 - Récupération complète d'un manga / jeu avec les bonnes informations 
- Récupérer un manga/jeu avec sa catégorie et ses auteurs/studios via une méthode repository utilisant Include

#### Test 6 - Flexing...
- Vous pouvez flex en rajoutant d'autres tests !!!