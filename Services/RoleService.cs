using Microsoft.EntityFrameworkCore;
using PNC.Data;
using PNC.Models;

namespace PNC.Services;

public interface IRoleService
{
    // Opérations de lecture
    Task<List<Role>> GetAllRolesAsync();
    Task<Role?> GetRoleByIdAsync(string id);
    Task<Role?> GetRoleByNomAsync(string nom);
    Task<List<Role>> GetRolesActifsAsync();
    Task<List<Role>> SearchRolesAsync(string searchTerm);
    Task<int> GetTotalRolesCountAsync();
    Task<List<Role>> GetRolesPaginatedAsync(int page, int pageSize);
    
    // Opérations d'écriture
    Task<Role?> CreateRoleAsync(Role role);
    Task<Role?> UpdateRoleAsync(Role role);
    Task<bool> DeleteRoleAsync(string id);
    Task<bool> ActiverRoleAsync(string id);
    Task<bool> DesactiverRoleAsync(string id);
    
    // Gestion des permissions
    Task<bool> AssignerPermissionAsync(string roleId, string permissionId);
    Task<bool> RetirerPermissionAsync(string roleId, string permissionId);
    Task<List<Permission>> GetPermissionsDuRoleAsync(string roleId);
    Task<bool> HasPermissionAsync(string roleId, string permissionNom);
    
    // Méthodes utilitaires
    string GenerateUniqueId();
    
    // Opérations de validation et vérification
    Task<bool> IsNomUniqueAsync(string nom, string? excludeId = null);
    Task<bool> RoleExistsAsync(string id);
    
    // Opérations de sauvegarde
    Task<bool> SaveChangesAsync();
}

public class RoleService : IRoleService
{
    private readonly IDbContextFactory<BdPolicePncContext> _contextFactory;

    public RoleService(IDbContextFactory<BdPolicePncContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    #region Opérations de lecture

    public async Task<List<Role>> GetAllRolesAsync()
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .OrderBy(r => r.Nom)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la récupération des rôles: {ex.Message}");
            return new List<Role>();
        }
    }

    public async Task<Role?> GetRoleByIdAsync(string id)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la récupération du rôle {id}: {ex.Message}");
            return null;
        }
    }

    public async Task<Role?> GetRoleByNomAsync(string nom)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Nom == nom);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la récupération du rôle {nom}: {ex.Message}");
            return null;
        }
    }

    public async Task<List<Role>> GetRolesActifsAsync()
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .Where(r => r.EstActif)
                .OrderBy(r => r.Nom)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la récupération des rôles actifs: {ex.Message}");
            return new List<Role>();
        }
    }

    public async Task<List<Role>> SearchRolesAsync(string searchTerm)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllRolesAsync();

            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .Where(r => r.Nom.Contains(searchTerm) || 
                           r.Description.Contains(searchTerm))
                .OrderBy(r => r.Nom)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la recherche de rôles: {ex.Message}");
            return new List<Role>();
        }
    }

    public async Task<int> GetTotalRolesCountAsync()
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Roles.CountAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors du comptage des rôles: {ex.Message}");
            return 0;
        }
    }

    public async Task<List<Role>> GetRolesPaginatedAsync(int page, int pageSize)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .OrderBy(r => r.Nom)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la pagination des rôles: {ex.Message}");
            return new List<Role>();
        }
    }

    #endregion

    #region Opérations d'écriture

    public async Task<Role?> CreateRoleAsync(Role role)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            // Générer un ID unique
            role.Id = GenerateUniqueId();
            
            // Définir la date de création
            role.DateCreation = DateTime.Now;
            
            // Ajouter le rôle
            context.Roles.Add(role);
            await context.SaveChangesAsync();
            
            return role;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la création du rôle: {ex.Message}");
            return null;
        }
    }

    public async Task<Role?> UpdateRoleAsync(Role role)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var existingRole = await context.Roles.FindAsync(role.Id);
            if (existingRole == null)
                return null;
            
            // Mettre à jour les propriétés
            existingRole.Nom = role.Nom;
            existingRole.Description = role.Description;
            existingRole.EstActif = role.EstActif;
            
            await context.SaveChangesAsync();
            return existingRole;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la mise à jour du rôle {role.Id}: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeleteRoleAsync(string id)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var role = await context.Roles.FindAsync(id);
            if (role == null)
                return false;
            
            // Vérifier si le rôle est utilisé par des utilisateurs
            var utilisateursAvecRole = await context.Utilisateurs
                .Where(u => u.IdRole == id)
                .CountAsync();
            
            if (utilisateursAvecRole > 0)
            {
                Console.WriteLine($"Impossible de supprimer le rôle {id}: utilisé par {utilisateursAvecRole} utilisateur(s)");
                return false;
            }
            
            // Supprimer les permissions du rôle
            var rolePermissions = await context.RolePermissions
                .Where(rp => rp.IdRole == id)
                .ToListAsync();
            
            context.RolePermissions.RemoveRange(rolePermissions);
            
            // Supprimer le rôle
            context.Roles.Remove(role);
            await context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la suppression du rôle {id}: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ActiverRoleAsync(string id)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var role = await context.Roles.FindAsync(id);
            if (role == null)
                return false;
            
            role.EstActif = true;
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'activation du rôle {id}: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DesactiverRoleAsync(string id)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var role = await context.Roles.FindAsync(id);
            if (role == null)
                return false;
            
            role.EstActif = false;
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la désactivation du rôle {id}: {ex.Message}");
            return false;
        }
    }

    #endregion

    #region Gestion des permissions

    public async Task<bool> AssignerPermissionAsync(string roleId, string permissionId)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            // Vérifier si la permission est déjà assignée
            var existingPermission = await context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.IdRole == roleId && rp.IdPermission == permissionId);
            
            if (existingPermission != null)
                return true; // Déjà assignée
            
            // Créer la nouvelle assignation
            var rolePermission = new RolePermission
            {
                IdRole = roleId,
                IdPermission = permissionId,
                DateAttribution = DateTime.Now
            };
            
            context.RolePermissions.Add(rolePermission);
            await context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'assignation de la permission: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RetirerPermissionAsync(string roleId, string permissionId)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var rolePermission = await context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.IdRole == roleId && rp.IdPermission == permissionId);
            
            if (rolePermission == null)
                return true; // Déjà retirée
            
            context.RolePermissions.Remove(rolePermission);
            await context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors du retrait de la permission: {ex.Message}");
            return false;
        }
    }

    public async Task<List<Permission>> GetPermissionsDuRoleAsync(string roleId)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.RolePermissions
                .Where(rp => rp.IdRole == roleId)
                .Select(rp => rp.Permission)
                .Where(p => p.EstActif)
                .OrderBy(p => p.Module)
                .ThenBy(p => p.Action)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la récupération des permissions du rôle {roleId}: {ex.Message}");
            return new List<Permission>();
        }
    }

    public async Task<bool> HasPermissionAsync(string roleId, string permissionNom)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.RolePermissions
                .AnyAsync(rp => rp.IdRole == roleId && 
                               rp.Permission.Nom == permissionNom && 
                               rp.Permission.EstActif);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la vérification de la permission: {ex.Message}");
            return false;
        }
    }

    #endregion

    #region Méthodes utilitaires

    public string GenerateUniqueId()
    {
        return Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
    }

    #endregion

    #region Opérations de validation et vérification

    public async Task<bool> IsNomUniqueAsync(string nom, string? excludeId = null)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var query = context.Roles.Where(r => r.Nom == nom);
            
            if (!string.IsNullOrEmpty(excludeId))
                query = query.Where(r => r.Id != excludeId);
            
            return !await query.AnyAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la vérification d'unicité du nom de rôle: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RoleExistsAsync(string id)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Roles.AnyAsync(r => r.Id == id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la vérification de l'existence du rôle: {ex.Message}");
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

