# Technical Context - PNC

## Technology Stack

### Backend Framework
- **.NET 9.0** : Framework principal avec support des dernières fonctionnalités
- **ASP.NET Core** : Plateforme web moderne et performante
- **Entity Framework Core 9.0.8** : ORM pour la gestion de la base de données
- **SQL Server** : Base de données relationnelle avec support des transactions

### Frontend Framework
- **Blazor Server** : Framework web interactif avec rendu côté serveur
- **Razor Components** : Composants réutilisables pour l'interface utilisateur
- **CSS** : Styles personnalisés avec fichiers .razor.css
- **JavaScript** : Interactivité côté client si nécessaire

### Architecture Patterns
- **N-Tier Architecture** : Séparation claire des responsabilités
- **Repository Pattern** : Abstraction de l'accès aux données
- **Service Layer Pattern** : Logique métier centralisée
- **Dependency Injection** : Gestion automatique des dépendances
- **Interface Segregation** : Interfaces spécifiques par responsabilité

## Development Setup

### Prerequisites
- **Visual Studio 2022** ou **VS Code** avec extensions .NET
- **.NET 9.0 SDK** installé
- **SQL Server** (local ou distant)
- **Git** pour le contrôle de version

### Database Configuration
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=JMK;Initial Catalog=bdPolicePNC;Integrated Security=True;Trust Server Certificate=True;MultipleActiveResultSets=True;"
  }
}
```

### Project Structure
```
PNC/
├── Components/           # Composants Blazor
│   ├── Pages/           # Pages de l'application
│   └── Layout/          # Mise en page et navigation
├── Data/                # Couche d'accès aux données
│   └── BdPolicePncContext.cs
├── Models/              # Modèles de données
├── Services/            # Services métier
├── wwwroot/             # Fichiers statiques
└── Program.cs           # Point d'entrée de l'application
```

## Technical Constraints

### Performance Requirements
- **Temps de réponse** : < 2 secondes pour les opérations CRUD
- **Concurrence** : Support de 100+ utilisateurs simultanés
- **Base de données** : Optimisation des requêtes avec Entity Framework
- **Pagination** : Gestion efficace des grandes listes

### Security Considerations
- **Authentification** : Système de connexion sécurisé
- **Autorisation** : Gestion des rôles et permissions
- **Validation** : Protection contre les injections et attaques
- **Audit** : Traçabilité de toutes les modifications

### Scalability
- **Architecture modulaire** : Ajout facile de nouvelles fonctionnalités
- **Services décomposés** : Évolution indépendante des composants
- **Base de données** : Support de la croissance des données
- **Cache** : Mise en cache des données fréquemment consultées

## Dependencies

### NuGet Packages
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.8" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.8" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.8" />
```

### External Dependencies
- **SQL Server** : Base de données principale
- **Windows Authentication** : Authentification intégrée
- **IIS/ASP.NET Core Hosting** : Hébergement web

## Development Workflow

### Code Organization
- **Services** : Logique métier et validation
- **Models** : Entités de données et DTOs
- **Components** : Interface utilisateur et logique de présentation
- **Data** : Accès aux données et contexte Entity Framework

### Coding Standards
- **Naming Conventions** : PascalCase pour les classes, camelCase pour les variables
- **Async/Await** : Utilisation systématique pour les opérations I/O
- **Error Handling** : Try-catch avec logging approprié
- **Documentation** : Commentaires XML pour les API publiques

### Testing Strategy
- **Unit Tests** : Tests des services métier
- **Integration Tests** : Tests de la couche de données
- **UI Tests** : Tests des composants Blazor
- **Performance Tests** : Tests de charge et de performance

## Deployment

### Environment Configuration
- **Development** : Configuration locale avec base de données de développement
- **Staging** : Environnement de test avec données de production
- **Production** : Environnement de production avec base de données SQL Server

### Build Process
- **Restore** : Récupération des packages NuGet
- **Build** : Compilation du projet
- **Test** : Exécution des tests automatisés
- **Publish** : Génération des artefacts de déploiement

### Monitoring
- **Logging** : Journalisation des opérations et erreurs
- **Performance** : Surveillance des temps de réponse
- **Errors** : Détection et alertes sur les erreurs
- **Usage** : Statistiques d'utilisation de l'application
