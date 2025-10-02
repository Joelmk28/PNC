# 🚔 PNC - Police Nationale Congolaise

## 📋 Description du Projet

Système de gestion des policiers de la Police Nationale Congolaise (PNC) développé avec **Blazor Server** et **Entity Framework Core**. Cette application permet la gestion complète des dossiers des policiers, incluant leurs informations personnelles, professionnelles, formations, et documents administratifs.

## 🏗️ Architecture

### Technologies Utilisées
- **.NET 9.0** - Framework principal
- **Blazor Server** - Interface utilisateur
- **Entity Framework Core 9.0.8** - ORM pour la base de données
- **SQL Server** - Base de données
- **Bootstrap Icons** - Icônes de l'interface

### Architecture N-Tier
```
📁 PNC/
├── 📁 Components/          # Composants Blazor
│   ├── 📁 Layout/         # Layouts principaux
│   └── 📁 Pages/          # Pages de l'application
├── 📁 Data/               # Couche d'accès aux données
│   └── BdPolicePncContext.cs
├── 📁 Models/             # Entités du domaine
├── 📁 Services/           # Couche métier
└── 📁 Middleware/         # Middleware personnalisé
```

## 🚀 Installation et Configuration

### Prérequis
- .NET 9.0 SDK
- SQL Server (LocalDB ou Express)
- Visual Studio 2022 ou VS Code

### Installation
1. **Cloner le repository**
   ```bash
   git clone [url-du-repo]
   cd PNC
   ```

2. **Restaurer les packages**
   ```bash
   dotnet restore
   ```

3. **Configurer la base de données**
   - Modifier la chaîne de connexion dans `appsettings.json`
   - Exécuter les migrations :
   ```bash
   dotnet ef database update
   ```

4. **Lancer l'application**
   ```bash
   dotnet run
   ```

## 🔧 Configuration

### Chaîne de Connexion
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BdPolicePnc;Trusted_Connection=true;MultipleActiveResultSets=true;Command Timeout=1200;"
  }
}
```

### Services Configurés
- `IPolicierService` - Gestion des policiers
- `ICommissariatService` - Gestion des commissariats
- `IUtilisateurService` - Gestion des utilisateurs
- `INutpService` - Gestion des numéros NUTP
- `CustomAuthenticationStateProvider` - Authentification personnalisée

## 📊 Fonctionnalités Principales

### 🔐 Authentification
- **Connexion sécurisée** avec nom d'utilisateur et mot de passe
- **Gestion des rôles** et permissions
- **Session persistante** jusqu'à déconnexion explicite

### 👮 Gestion des Policiers

#### Formulaire Multi-Étapes (14 étapes)
1. **Identification de base** - NUTP, commissariat, informations personnelles
2. **Informations professionnelles** - Grade, fonction, statut
3. **État civil** - Conjoints et informations familiales
4. **Enfants** - Gestion des enfants
5. **Origine et études** - Formation académique et professionnelle
6. **Contact** - Adresses et moyens de communication
7. **Langues** - Langues parlées (nationales et internationales)
8. **Sports** - Activités sportives
9. **Distinctions** - Récompenses et honneurs
10. **Permis** - Permis de conduire
11. **Médical** - Informations de santé
12. **Unités** - Affectations
13. **Documents** - Pièces administratives
14. **Urgence** - Personnes à contacter

#### Vérification NUTP
- **Vérification automatique** de la disponibilité du numéro NUTP
- **Statuts** : `FREE` (disponible) / `BUSY` (utilisé)
- **Blocage des champs** jusqu'à vérification réussie

### 🏢 Gestion des Commissariats
- **CRUD complet** des commissariats
- **Géolocalisation** par province et territoire
- **Statistiques** par commissariat

### 📈 Tableau de Bord
- **Statistiques générales** des policiers
- **Graphiques** de répartition par grade, statut, commissariat
- **Indicateurs** de performance

## 🗄️ Modèle de Données

### Entités Principales
- **Policier** - Informations complètes du policier
- **Commissariat** - Unités administratives
- **Utilisateur** - Comptes d'accès système
- **Nutp** - Numéros uniques de traitement policier
- **Formation** - Études et formations
- **Conjoint** - Informations du conjoint
- **Enfant** - Informations des enfants

### Relations
- Un policier appartient à un commissariat
- Un policier peut avoir plusieurs formations, conjoints, enfants
- Un utilisateur a un rôle avec des permissions spécifiques

## 🎨 Interface Utilisateur

### Design Moderne
- **Interface responsive** adaptée à tous les écrans
- **Thème professionnel** aux couleurs de la PNC
- **Navigation intuitive** avec étapes visuelles
- **Validation en temps réel** des formulaires

### Composants Clés
- **Formulaire multi-étapes** avec navigation fluide
- **Tableaux de données** avec pagination
- **Modales** pour les actions rapides
- **Messages de validation** contextuels

## 🔒 Sécurité

### Authentification
- **Système de connexion** personnalisé
- **Gestion des sessions** sécurisée
- **Protection des routes** sensibles

### Validation
- **Validation côté client** et serveur
- **Vérification des permissions** par action
- **Sanitisation** des données d'entrée

## 🧪 Tests

### Types de Tests
- **Tests unitaires** des services métier
- **Tests d'intégration** de la base de données
- **Tests d'interface** des composants Blazor

### Exécution des Tests
```bash
dotnet test
```

## 📦 Déploiement

### Environnement de Production
1. **Configuration** de la base de données de production
2. **Optimisation** des performances
3. **Sécurisation** des connexions
4. **Monitoring** des erreurs

### Docker (Optionnel)
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0
COPY . /app
WORKDIR /app
EXPOSE 80
ENTRYPOINT ["dotnet", "PNC.dll"]
```

## 🤝 Contribution

### Guidelines
1. **Fork** le projet
2. **Créer** une branche feature (`git checkout -b feature/nouvelle-fonctionnalite`)
3. **Commit** les changements (`git commit -am 'Ajout nouvelle fonctionnalité'`)
4. **Push** vers la branche (`git push origin feature/nouvelle-fonctionnalite`)
5. **Créer** une Pull Request

### Standards de Code
- **C#** : Suivre les conventions Microsoft
- **Blazor** : Composants réutilisables et modulaires
- **CSS** : Classes BEM et responsive design
- **Commentaires** : Documentation claire des méthodes complexes

## 📝 Changelog

### Version 1.0.0
- ✅ Système d'authentification complet
- ✅ Gestion des policiers avec formulaire multi-étapes
- ✅ Vérification NUTP en temps réel
- ✅ Interface utilisateur moderne et responsive
- ✅ Gestion des commissariats et utilisateurs
- ✅ Tableau de bord avec statistiques

## 🐛 Issues Connues

- [ ] Tests automatisés à implémenter
- [ ] Logging avancé à configurer
- [ ] Export/Import des données
- [ ] Notifications en temps réel

## 📞 Support

### Contact
- **Développeur** : [Votre nom]
- **Email** : [votre-email@domain.com]
- **Organisation** : Police Nationale Congolaise

### Documentation
- **API** : [Lien vers la documentation API]
- **Guide utilisateur** : [Lien vers le guide]
- **FAQ** : [Lien vers les questions fréquentes]

## 📄 Licence

Ce projet est développé pour la **Police Nationale Congolaise** et est destiné à un usage interne. Tous droits réservés.

---

## 🏆 Remerciements

- **Police Nationale Congolaise** pour la confiance accordée
- **Équipe de développement** pour le travail accompli
- **Communauté .NET** pour les ressources et le support

---

*Développé avec ❤️ pour la Police Nationale Congolaise*
