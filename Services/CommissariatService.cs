using Microsoft.EntityFrameworkCore;
using PNC.Data;
using PNC.Models;

namespace PNC.Services;

public interface ICommissariatService
{
    // Opérations de lecture
    Task<List<Commissariat>> GetAllCommissariatsAsync();
    Task<Commissariat?> GetCommissariatByIdAsync(string id);
    Task<Commissariat?> GetCommissariatByCodeAsync(string code);
    Task<Commissariat?> GetCommissariatByNameAsync(string nom);
    Task<List<Commissariat>> SearchCommissariatsAsync(string searchTerm);
    Task<List<Commissariat>> GetCommissariatsByProvinceAsync(string province);
    Task<List<Commissariat>> GetCommissariatsByTerritoireAsync(string territoire);
    Task<int> GetTotalCommissariatsCountAsync();
    Task<List<Commissariat>> GetCommissariatsPaginatedAsync(int page, int pageSize);
    
    // Opérations d'écriture
    Task<bool> CreateCommissariatAsync(Commissariat commissariat);
    Task<bool> UpdateCommissariatAsync(Commissariat commissariat);
    Task<bool> DeleteCommissariatAsync(string id);
    
    // Opérations sur les policiers affectés
    Task<List<Policier>> GetPoliciersByCommissariatAsync(string commissariatId);
    Task<int> GetPoliciersCountByCommissariatAsync(string commissariatId);
    Task<bool> AssignPolicierToCommissariatAsync(string policierId, string commissariatId);
    Task<bool> RemovePolicierFromCommissariatAsync(string policierId);
    
    // Opérations de validation et vérification
    Task<bool> IsCodeUniqueAsync(string code, string? excludeId = null);
    Task<bool> IsNameUniqueAsync(string nom, string? excludeId = null);
    Task<bool> CommissariatExistsAsync(string id);
    
    // Opérations de sauvegarde
    Task<bool> SaveChangesAsync();
    
    // Statistiques optimisées
    Task<CommissariatStatistics> GetCommissariatStatisticsAsync(string commissariatId);
    Task<List<CommissariatStatistics>> GetAllCommissariatsStatisticsAsync();
    
    // Nouvelles méthodes optimisées
    Task<CommissariatsOverview> GetCommissariatsOverviewAsync();
    Task<Dictionary<string, int>> GetPoliciersCountByCommissariatAsync();
}

public class CommissariatService : ICommissariatService
{
    private readonly IDbContextFactory<BdPolicePncContext> _contextFactory;

    public CommissariatService(IDbContextFactory<BdPolicePncContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    #region Opérations de lecture

    public async Task<List<Commissariat>> GetAllCommissariatsAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Commissariats
            .OrderBy(c => c.Nom ?? string.Empty)
            .ToListAsync();
    }

    public async Task<Commissariat?> GetCommissariatByIdAsync(string id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Commissariats
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Commissariat?> GetCommissariatByCodeAsync(string code)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Commissariats
            .FirstOrDefaultAsync(c => c.Nom == code);
    }

    public async Task<Commissariat?> GetCommissariatByNameAsync(string nom)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Commissariats
            .FirstOrDefaultAsync(c => c.Nom == nom);
    }

    public async Task<List<Commissariat>> SearchCommissariatsAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return new List<Commissariat>();

        using var context = await _contextFactory.CreateDbContextAsync();
        var term = searchTerm.ToLower();
        
        return await context.Commissariats
            .Where(c => (c.Nom != null && c.Nom.ToLower().Contains(term)) ||
                       (c.Province != null && c.Province.ToLower().Contains(term)) ||
                       (c.Territoire != null && c.Territoire.ToLower().Contains(term)) ||
                       (c.District != null && c.District.ToLower().Contains(term)))
            .OrderBy(c => c.Nom ?? string.Empty)
            .ToListAsync();
    }

    public async Task<List<Commissariat>> GetCommissariatsByProvinceAsync(string province)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Commissariats
            .Where(c => c.Province == province)
            .OrderBy(c => c.Nom ?? string.Empty)
            .ToListAsync();
    }

    public async Task<List<Commissariat>> GetCommissariatsByTerritoireAsync(string territoire)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Commissariats
            .Where(c => c.Territoire == territoire)
            .OrderBy(c => c.Nom ?? string.Empty)
            .ToListAsync();
    }

    public async Task<int> GetTotalCommissariatsCountAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Commissariats.CountAsync();
    }

    public async Task<List<Commissariat>> GetCommissariatsPaginatedAsync(int page, int pageSize)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Commissariats
            .OrderBy(c => c.Nom ?? string.Empty)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    #endregion

    #region Opérations d'écriture

    public async Task<bool> CreateCommissariatAsync(Commissariat commissariat)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            // Générer un ID unique si pas déjà défini
            if (string.IsNullOrEmpty(commissariat.Id))
                commissariat.Id = GenerateShortId();
            
            context.Commissariats.Add(commissariat);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la création du commissariat: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateCommissariatAsync(Commissariat commissariat)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var existingCommissariat = await context.Commissariats
                .FirstOrDefaultAsync(c => c.Id == commissariat.Id);
            
            if (existingCommissariat == null)
                return false;
            
            context.Entry(existingCommissariat).CurrentValues.SetValues(commissariat);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la mise à jour du commissariat: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteCommissariatAsync(string id)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var commissariat = await context.Commissariats
                .FirstOrDefaultAsync(c => c.Id == id);
            
            if (commissariat == null)
                return false;
            
            // Vérifier s'il y a des policiers affectés
            var policiersCount = await context.Policiers
                .CountAsync(p => p.NomUnite == commissariat.Nom);
            
            if (policiersCount > 0)
            {
                Console.WriteLine($"Impossible de supprimer le commissariat: {policiersCount} policiers y sont affectés");
                return false;
            }
            
            context.Commissariats.Remove(commissariat);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la suppression du commissariat: {ex.Message}");
            return false;
        }
    }

    #endregion

    #region Opérations sur les policiers affectés

    public async Task<List<Policier>> GetPoliciersByCommissariatAsync(string commissariatId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        
        var commissariat = await context.Commissariats.FindAsync(commissariatId);
        if (commissariat == null) return new List<Policier>();
        
        return await context.Policiers
            .Where(p => p.NomUnite == commissariat.Nom)
            .OrderBy(p => p.Nom)
            .ThenBy(p => p.Prenom)
            .ToListAsync();
    }

    public async Task<int> GetPoliciersCountByCommissariatAsync(string commissariatId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        
        var commissariat = await context.Commissariats.FindAsync(commissariatId);
        if (commissariat == null) return 0;
        
        return await context.Policiers
            .CountAsync(p => p.NomUnite == commissariat.Nom);
    }

    public async Task<bool> AssignPolicierToCommissariatAsync(string policierId, string commissariatId)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var policier = await context.Policiers.FindAsync(policierId);
            var commissariat = await context.Commissariats.FindAsync(commissariatId);
            
            if (policier == null || commissariat == null) return false;
            
            policier.NomUnite = commissariat.Nom;
            
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'affectation du policier: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RemovePolicierFromCommissariatAsync(string policierId)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var policier = await context.Policiers.FindAsync(policierId);
            if (policier == null) return false;
            
            policier.UniteAffectation = string.Empty;
            policier.NomUnite = string.Empty;
            
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la désaffectation du policier: {ex.Message}");
            return false;
        }
    }

    #endregion

    #region Opérations de validation et vérification

    public async Task<bool> IsCodeUniqueAsync(string code, string? excludeId = null)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        
        if (string.IsNullOrEmpty(excludeId))
            return !await context.Commissariats.AnyAsync(c => c.Nom == code);
        
        return !await context.Commissariats.AnyAsync(c => c.Nom == code && c.Id != excludeId);
    }

    public async Task<bool> IsNameUniqueAsync(string nom, string? excludeId = null)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        
        if (string.IsNullOrEmpty(excludeId))
            return !await context.Commissariats.AnyAsync(c => c.Nom == nom);
        
        return !await context.Commissariats.AnyAsync(c => c.Nom == nom && c.Id != excludeId);
    }

    public async Task<bool> CommissariatExistsAsync(string id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Commissariats.AnyAsync(c => c.Id == id);
    }

    #endregion

    #region Opérations de sauvegarde

    public async Task<bool> SaveChangesAsync()
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la sauvegarde: {ex.Message}");
            return false;
        }
    }

    #endregion

    #region Statistiques optimisées

    public async Task<CommissariatStatistics> GetCommissariatStatisticsAsync(string commissariatId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        
        var commissariat = await context.Commissariats.FindAsync(commissariatId);
        if (commissariat == null) return new CommissariatStatistics();
        
        var policiers = await context.Policiers
            .Where(p => p.NomUnite == commissariat.Nom)
            .ToListAsync();
        
        var totalPoliciers = policiers.Count;
        var activePoliciers = policiers.Count(p => p.DateEntreePolice <= DateTime.Today);
        var newThisMonth = policiers.Count(p => 
            p.DateEntreePolice.Month == DateTime.Today.Month && 
            p.DateEntreePolice.Year == DateTime.Today.Year);
        
        var gradeDistribution = policiers
            .GroupBy(p => p.NatureGrade)
            .ToDictionary(g => g.Key ?? "Non défini", g => g.Count());
        
        var genderDistribution = policiers
            .GroupBy(p => p.Sexe)
            .ToDictionary(g => g.Key ?? "Non défini", g => g.Count());
        
        return new CommissariatStatistics
        {
            CommissariatId = commissariatId,
            CommissariatName = commissariat.Nom,
            TotalPoliciers = totalPoliciers,
            ActivePoliciers = activePoliciers,
            NewThisMonth = newThisMonth,
            GradeDistribution = gradeDistribution,
            GenderDistribution = genderDistribution
        };
    }

    public async Task<List<CommissariatStatistics>> GetAllCommissariatsStatisticsAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        
        var commissariats = await context.Commissariats.ToListAsync();
        var result = new List<CommissariatStatistics>();
        
        foreach (var commissariat in commissariats)
        {
            var stats = await GetCommissariatStatisticsAsync(commissariat.Id);
            result.Add(stats);
        }
        
        return result.OrderByDescending(s => s.TotalPoliciers).ToList();
    }

    #endregion

    #region Nouvelles méthodes optimisées

    /// <summary>
    /// Récupère un aperçu complet des commissariats en une seule requête
    /// </summary>
    public async Task<CommissariatsOverview> GetCommissariatsOverviewAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        
        try
        {
            // Récupérer tous les commissariats
            var commissariats = await context.Commissariats
                .OrderBy(c => c.Nom ?? string.Empty)
                .ToListAsync();

            // Récupérer le nombre de policiers par commissariat en une seule requête
            var policiersCounts = await context.Policiers
                .GroupBy(p => p.NomUnite)
                .Where(g => !string.IsNullOrEmpty(g.Key))
                .Select(g => new { CommissariatNom = g.Key, Count = g.Count() })
                .ToListAsync();

            // Créer un dictionnaire pour un accès rapide
            var policiersCountDict = policiersCounts.ToDictionary(x => x.CommissariatNom, x => x.Count);

            // Calculer les statistiques
            var totalCommissariats = commissariats.Count;
            var provinces = commissariats.Select(c => c.Province).Where(p => !string.IsNullOrEmpty(p)).Distinct().Count();
            var territoires = commissariats.Select(c => c.Territoire).Where(t => !string.IsNullOrEmpty(t)).Distinct().Count();
            var totalPoliciers = policiersCounts.Sum(x => x.Count);

            return new CommissariatsOverview
            {
                Commissariats = commissariats,
                TotalCommissariats = totalCommissariats,
                TotalPoliciers = totalPoliciers,
                ProvincesCount = provinces,
                TerritoiresCount = territoires,
                PoliciersCountByCommissariat = policiersCountDict
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la récupération de l'aperçu: {ex.Message}");
            return new CommissariatsOverview();
        }
    }

    /// <summary>
    /// Récupère le nombre de policiers par commissariat en une seule requête
    /// </summary>
    public async Task<Dictionary<string, int>> GetPoliciersCountByCommissariatAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        
        try
        {
            var result = await context.Policiers
                .GroupBy(p => p.NomUnite)
                .Where(g => !string.IsNullOrEmpty(g.Key))
                .Select(g => new { CommissariatNom = g.Key, Count = g.Count() })
                .ToListAsync();

            return result.ToDictionary(x => x.CommissariatNom, x => x.Count);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la récupération du nombre de policiers: {ex.Message}");
            return new Dictionary<string, int>();
        }
    }

    #endregion
    
    /// <summary>
    /// Génère un ID unique de 10 caractères pour les commissariats
    /// </summary>
    private string GenerateShortId()
    {
        // Utiliser les 8 premiers caractères du GUID + 2 caractères aléatoires
        var guid = Guid.NewGuid().ToString("N"); // Format sans tirets
        var random = new Random();
        var randomChars = new string(Enumerable.Range(0, 2)
            .Select(_ => (char)random.Next('A', 'Z' + 1))
            .ToArray());
        
        return guid.Substring(0, 8) + randomChars;
    }
}

#region Modèles de données pour les statistiques

public class CommissariatStatistics
{
    public string CommissariatId { get; set; } = string.Empty;
    public string CommissariatName { get; set; } = string.Empty;
    public int TotalPoliciers { get; set; }
    public int ActivePoliciers { get; set; }
    public int NewThisMonth { get; set; }
    public Dictionary<string, int> GradeDistribution { get; set; } = new();
    public Dictionary<string, int> GenderDistribution { get; set; } = new();
}

/// <summary>
/// Modèle optimisé pour récupérer toutes les données en une seule fois
/// </summary>
public class CommissariatsOverview
{
    public List<Commissariat> Commissariats { get; set; } = new();
    public int TotalCommissariats { get; set; }
    public int TotalPoliciers { get; set; }
    public int ProvincesCount { get; set; }
    public int TerritoiresCount { get; set; }
    public Dictionary<string, int> PoliciersCountByCommissariat { get; set; } = new();
}

#endregion
