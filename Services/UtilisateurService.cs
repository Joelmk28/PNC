using Microsoft.EntityFrameworkCore;
using PNC.Data;
using PNC.Models;
using System.Security.Cryptography;
using System.Text;

namespace PNC.Services;

public interface IUtilisateurService
{
    // Opérations de lecture
    Task<List<Utilisateur>> GetAllUtilisateursAsync();
    Task<Utilisateur?> GetUtilisateurByIdAsync(string id);
    Task<Utilisateur?> GetUtilisateurByNomUtilisateurAsync(string nomUtilisateur);
    Task<Utilisateur?> GetUtilisateurByEmailAsync(string email);
    Task<List<Utilisateur>> SearchUtilisateursAsync(string searchTerm);
    Task<List<Utilisateur>> GetUtilisateursByRoleAsync(string roleId);
    Task<List<Utilisateur>> GetUtilisateursActifsAsync();
    Task<int> GetTotalUtilisateursCountAsync();
    Task<List<Utilisateur>> GetUtilisateursPaginatedAsync(int page, int pageSize);
    
    // Opérations d'écriture
    Task<Utilisateur?> CreateUtilisateurAsync(Utilisateur utilisateur, string motDePasse);
    Task<Utilisateur?> UpdateUtilisateurAsync(Utilisateur utilisateur);
    Task<bool> DeleteUtilisateurAsync(string id);
    Task<bool> ActiverUtilisateurAsync(string id);
    Task<bool> DesactiverUtilisateurAsync(string id);
    
    // Gestion des mots de passe
    Task<bool> ChangeMotDePasseAsync(string utilisateurId, string ancienMotDePasse, string nouveauMotDePasse);
    Task<bool> ResetMotDePasseAsync(string utilisateurId, string nouveauMotDePasse);
    Task<bool> VerifierMotDePasseAsync(string utilisateurId, string motDePasse);
    
    // Gestion des rôles
    Task<bool> AssignerRoleAsync(string utilisateurId, string roleId);
    Task<bool> RetirerRoleAsync(string utilisateurId);
    
    // Authentification et connexion
    Task<Utilisateur?> AuthentifierAsync(string nomUtilisateur, string motDePasse);
    Task<bool> EnregistrerConnexionAsync(string utilisateurId);
    
    // Méthodes utilitaires
    string GenerateUniqueId();
    string HasherMotDePasse(string motDePasse);
    bool VerifierMotDePasse(string motDePasse, string hash);
    
    // Opérations de validation et vérification
    Task<bool> IsNomUtilisateurUniqueAsync(string nomUtilisateur, string? excludeId = null);
    Task<bool> IsEmailUniqueAsync(string email, string? excludeId = null);
    Task<bool> UtilisateurExistsAsync(string id);
    
    // Opérations de sauvegarde
    Task<bool> SaveChangesAsync();
}

public class UtilisateurService : IUtilisateurService
{
    private readonly IDbContextFactory<BdPolicePncContext> _contextFactory;

    

    public UtilisateurService(IDbContextFactory<BdPolicePncContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    #region Opérations de lecture

    public async Task<List<Utilisateur>> GetAllUtilisateursAsync()
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Utilisateurs
                .Include(u => u.IdRoleNavigation)
                .Include(u => u.IdEnqueteurNavigation)
                .OrderBy(u => u.Nom)
                .ThenBy(u => u.Prenom)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la récupération des utilisateurs: {ex.Message}");
            return new List<Utilisateur>();
        }
    }

    public async Task<Utilisateur?> GetUtilisateurByIdAsync(string id)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Utilisateurs
                .Include(u => u.IdRoleNavigation)
                .Include(u => u.IdEnqueteurNavigation)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la récupération de l'utilisateur {id}: {ex.Message}");
            return null;
        }
    }

    public async Task<Utilisateur?> GetUtilisateurByNomUtilisateurAsync(string nomUtilisateur)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Utilisateurs
                .Include(u => u.IdRoleNavigation)
                .Include(u => u.IdEnqueteurNavigation)
                .FirstOrDefaultAsync(u => u.NomUtilisateur == nomUtilisateur);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la récupération de l'utilisateur {nomUtilisateur}: {ex.Message}");
            return null;
        }
    }

    public async Task<Utilisateur?> GetUtilisateurByEmailAsync(string email)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Utilisateurs
                .Include(u => u.IdRoleNavigation)
                .Include(u => u.IdEnqueteurNavigation)
                .FirstOrDefaultAsync(u => u.Email == email);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la récupération de l'utilisateur par email {email}: {ex.Message}");
            return null;
        }
    }

    public async Task<List<Utilisateur>> SearchUtilisateursAsync(string searchTerm)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllUtilisateursAsync();

            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Utilisateurs
                .Include(u => u.IdRoleNavigation)
                .Include(u => u.IdEnqueteurNavigation)
                .Where(u => u.Nom.Contains(searchTerm) || 
                           u.Prenom.Contains(searchTerm) || 
                           u.NomUtilisateur.Contains(searchTerm) || 
                           u.Email.Contains(searchTerm))
                .OrderBy(u => u.Nom)
                .ThenBy(u => u.Prenom)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la recherche d'utilisateurs: {ex.Message}");
            return new List<Utilisateur>();
        }
    }

    public async Task<List<Utilisateur>> GetUtilisateursByRoleAsync(string roleId)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Utilisateurs
                .Include(u => u.IdRoleNavigation)
                .Include(u => u.IdEnqueteurNavigation)
                .Where(u => u.IdRole == roleId)
                .OrderBy(u => u.Nom)
                .ThenBy(u => u.Prenom)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la récupération des utilisateurs par rôle {roleId}: {ex.Message}");
            return new List<Utilisateur>();
        }
    }

    public async Task<List<Utilisateur>> GetUtilisateursActifsAsync()
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Utilisateurs
                .Include(u => u.IdRoleNavigation)
                .Include(u => u.IdEnqueteurNavigation)
                .Where(u => u.EstActif)
                .OrderBy(u => u.Nom)
                .ThenBy(u => u.Prenom)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la récupération des utilisateurs actifs: {ex.Message}");
            return new List<Utilisateur>();
        }
    }

    public async Task<int> GetTotalUtilisateursCountAsync()
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Utilisateurs.CountAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors du comptage des utilisateurs: {ex.Message}");
            return 0;
        }
    }

    public async Task<List<Utilisateur>> GetUtilisateursPaginatedAsync(int page, int pageSize)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Utilisateurs
                .Include(u => u.IdRoleNavigation)
                .Include(u => u.IdEnqueteurNavigation)
                .OrderBy(u => u.Nom)
                .ThenBy(u => u.Prenom)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la pagination des utilisateurs: {ex.Message}");
            return new List<Utilisateur>();
        }
    }

    #endregion

    #region Opérations d'écriture

    public async Task<Utilisateur?> CreateUtilisateurAsync(Utilisateur utilisateur, string motDePasse)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            // Générer un ID unique
            utilisateur.Id = GenerateUniqueId();
            
            // Hasher le mot de passe
            utilisateur.MotDePasse = HasherMotDePasse(motDePasse);
            
            // Définir la date de création
            utilisateur.DateCreation = DateTime.Now;
            
            // Ajouter l'utilisateur
            context.Utilisateurs.Add(utilisateur);
            await context.SaveChangesAsync();
            
            return utilisateur;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la création de l'utilisateur: {ex.Message}");
            return null;
        }
    }

    public async Task<Utilisateur?> UpdateUtilisateurAsync(Utilisateur utilisateur)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var existingUser = await context.Utilisateurs.FindAsync(utilisateur.Id);
            if (existingUser == null)
                return null;
            
            // Mettre à jour les propriétés
            existingUser.Nom = utilisateur.Nom;
            existingUser.Prenom = utilisateur.Prenom;
            existingUser.Email = utilisateur.Email;
            existingUser.Telephone = utilisateur.Telephone;
            existingUser.EstActif = utilisateur.EstActif;
            existingUser.IdRole = utilisateur.IdRole;
            existingUser.IdEnqueteur = utilisateur.IdEnqueteur;
            
            await context.SaveChangesAsync();
            return existingUser;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la mise à jour de l'utilisateur {utilisateur.Id}: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeleteUtilisateurAsync(string id)
    { 
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var utilisateur = await context.Utilisateurs.FindAsync(id);
            if (utilisateur == null)
                return false;
            
            context.Utilisateurs.Remove(utilisateur);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la suppression de l'utilisateur {id}: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ActiverUtilisateurAsync(string id)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var utilisateur = await context.Utilisateurs.FindAsync(id);
            if (utilisateur == null)
                return false;
            
            utilisateur.EstActif = true;
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'activation de l'utilisateur {id}: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DesactiverUtilisateurAsync(string id)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var utilisateur = await context.Utilisateurs.FindAsync(id);
            if (utilisateur == null)
                return false;
            
            utilisateur.EstActif = false;
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la désactivation de l'utilisateur {id}: {ex.Message}");
            return false;
        }
    }

    #endregion

    #region Gestion des mots de passe

    public async Task<bool> ChangeMotDePasseAsync(string utilisateurId, string ancienMotDePasse, string nouveauMotDePasse)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var utilisateur = await context.Utilisateurs.FindAsync(utilisateurId);
            if (utilisateur == null)
                return false;
            
            // Vérifier l'ancien mot de passe
            if (!VerifierMotDePasse(ancienMotDePasse, utilisateur.MotDePasse))
                return false;
            
            // Hasher et sauvegarder le nouveau mot de passe
            utilisateur.MotDePasse = HasherMotDePasse(nouveauMotDePasse);
            await context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors du changement de mot de passe: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ResetMotDePasseAsync(string utilisateurId, string nouveauMotDePasse)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var utilisateur = await context.Utilisateurs.FindAsync(utilisateurId);
            if (utilisateur == null)
                return false;
            
            // Hasher et sauvegarder le nouveau mot de passe
            utilisateur.MotDePasse = HasherMotDePasse(nouveauMotDePasse);
            await context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la réinitialisation du mot de passe: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> VerifierMotDePasseAsync(string utilisateurId, string motDePasse)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var utilisateur = await context.Utilisateurs.FindAsync(utilisateurId);
            if (utilisateur == null)
                return false;
            
            return VerifierMotDePasse(motDePasse, utilisateur.MotDePasse);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la vérification du mot de passe: {ex.Message}");
            return false;
        }
    }

    #endregion

    #region Gestion des rôles

    public async Task<bool> AssignerRoleAsync(string utilisateurId, string roleId)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var utilisateur = await context.Utilisateurs.FindAsync(utilisateurId);
            if (utilisateur == null)
                return false;
            
            var role = await context.Roles.FindAsync(roleId);
            if (role == null)
                return false;
            
            utilisateur.IdRole = roleId;
            await context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'assignation du rôle: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RetirerRoleAsync(string utilisateurId)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var utilisateur = await context.Utilisateurs.FindAsync(utilisateurId);
            if (utilisateur == null)
                return false;
            
            utilisateur.IdRole = null;
            await context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors du retrait du rôle: {ex.Message}");
            return false;
        }
    }

    #endregion

    #region Authentification et connexion

    public async Task<Utilisateur?> AuthentifierAsync(string nomUtilisateur, string motDePasse)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var utilisateur = await context.Utilisateurs
                .Include(u => u.IdRoleNavigation)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.NomUtilisateur == nomUtilisateur && u.EstActif);
           
            if (utilisateur == null)
            {
                return null;
            }    
               
            
            if (!VerifierMotDePasse(motDePasse, utilisateur.MotDePasse))
            {
                return null;
            }
               
      
            // Enregistrer la connexion
            utilisateur.DerniereConnexion = DateTime.Now;
            //await context.SaveChangesAsync();
           
            
            return utilisateur;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'authentification: {ex.Message}");
           
            return null;
        }
    }

    public async Task<bool> EnregistrerConnexionAsync(string utilisateurId)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var utilisateur = await context.Utilisateurs.FindAsync(utilisateurId);
            if (utilisateur == null)
                return false;
            
            utilisateur.DerniereConnexion = DateTime.Now;
            await context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'enregistrement de la connexion: {ex.Message}");
            return false;
        }
    }

    #endregion

    #region Méthodes utilitaires

    public string GenerateUniqueId()
    {
        return Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
    }

    public string HasherMotDePasse(string motDePasse)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(motDePasse));
        return Convert.ToBase64String(hashedBytes);
    }

    public bool VerifierMotDePasse(string motDePasse, string hash)
    {
        var hashToVerify = HasherMotDePasse(motDePasse);
        return hashToVerify == hash;
    }

    #endregion

    #region Opérations de validation et vérification

    public async Task<bool> IsNomUtilisateurUniqueAsync(string nomUtilisateur, string? excludeId = null)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var query = context.Utilisateurs.Where(u => u.NomUtilisateur == nomUtilisateur);
            
            if (!string.IsNullOrEmpty(excludeId))
                query = query.Where(u => u.Id != excludeId);
            
            return !await query.AnyAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la vérification d'unicité du nom d'utilisateur: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> IsEmailUniqueAsync(string email, string? excludeId = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
                return true;
            
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var query = context.Utilisateurs.Where(u => u.Email == email);
            
            if (!string.IsNullOrEmpty(excludeId))
                query = query.Where(u => u.Id != excludeId);
            
            return !await query.AnyAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la vérification d'unicité de l'email: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UtilisateurExistsAsync(string id)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Utilisateurs.AnyAsync(u => u.Id == id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la vérification de l'existence de l'utilisateur: {ex.Message}");
            return false;
        }
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
}

