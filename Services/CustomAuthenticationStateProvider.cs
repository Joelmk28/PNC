using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using PNC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace PNC.Services;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IUtilisateurService _utilisateurService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private Utilisateur? _currentUser;

    public CustomAuthenticationStateProvider(IUtilisateurService utilisateurService, IHttpContextAccessor httpContextAccessor)
    {
        _utilisateurService = utilisateurService;
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            // Si un utilisateur est déjà en mémoire, l'utiliser
            if (_currentUser != null)
            {
                var claims = CreateUserClaims(_currentUser);
                var identity = new ClaimsIdentity(claims, "CustomAuth");
                var principal = new ClaimsPrincipal(identity);
                return new AuthenticationState(principal);
            }

            // Aucun utilisateur authentifié
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la récupération de l'état d'authentification: {ex.Message}");
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    public async Task<AuthenticationResult> SignInAsync(string username, string password)
    {
        try
        {
            // Authentifier l'utilisateur via le service
            var utilisateur = await _utilisateurService.AuthentifierAsync(username, password);
            
            if (utilisateur == null)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    ErrorMessage = "Nom d'utilisateur ou mot de passe incorrect"
                };
            }

            if (!utilisateur.EstActif)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    ErrorMessage = "Compte utilisateur désactivé"
                };
            }

            // Créer les claims utilisateur
            var claims = CreateUserClaims(utilisateur);
            var identity = new ClaimsIdentity(claims, "CustomAuth");
            var principal = new ClaimsPrincipal(identity);

            // Stocker l'utilisateur en mémoire (session)
            _currentUser = utilisateur;

            // Notifier le changement d'état d'authentification
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));

            return new AuthenticationResult
            {
                Success = true,
                User = utilisateur
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la connexion: {ex.Message}");
            return new AuthenticationResult
            {
                Success = false,
                ErrorMessage = "Erreur de connexion. Veuillez réessayer."
            };
        }
    }

    public async Task SignOutAsync()
    {
        try
        {
            // Réinitialiser l'utilisateur courant
            _currentUser = null;

            // Créer un état d'authentification vide
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = new AuthenticationState(anonymousUser);

            // Notifier le changement d'état
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la déconnexion: {ex.Message}");
        }
    }

    public Task<Utilisateur?> GetCurrentUserAsync()
    {
        return Task.FromResult(_currentUser);
    }

    public Task<bool> IsUserInRoleAsync(string roleName)
    {
        try
        {
            if (_currentUser?.IdRoleNavigation?.Nom == roleName)
                return Task.FromResult(true);

            return Task.FromResult(false);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la vérification du rôle: {ex.Message}");
            return Task.FromResult(false);
        }
    }

    public Task<bool> HasPermissionAsync(string permissionName)
    {
        try
        {
            if (_currentUser?.IdRoleNavigation?.RolePermissions == null)
                return Task.FromResult(false);

            var hasPermission = _currentUser.IdRoleNavigation.RolePermissions
                .Any(rp => rp.Permission?.Nom == permissionName);
            
            return Task.FromResult(hasPermission);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la vérification de la permission: {ex.Message}");
            return Task.FromResult(false);
        }
    }

    private List<Claim> CreateUserClaims(Utilisateur utilisateur)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, utilisateur.Id),
            new Claim(ClaimTypes.Name, utilisateur.NomUtilisateur),
            new Claim(ClaimTypes.GivenName, utilisateur.Prenom ?? ""),
            new Claim(ClaimTypes.Surname, utilisateur.Nom),
            new Claim(ClaimTypes.Email, utilisateur.Email ?? ""),
            new Claim(ClaimTypes.Role, utilisateur.IdRoleNavigation?.Nom ?? ""),
            new Claim("UserFullName", $"{utilisateur.Nom} {utilisateur.Prenom}".Trim()),
            new Claim("UserId", utilisateur.Id),
            new Claim("UserRole", utilisateur.IdRoleNavigation?.Nom ?? ""),
            new Claim("UserActive", utilisateur.EstActif.ToString())
        };

        // Ajouter les permissions spécifiques si disponibles
        if (utilisateur.IdRoleNavigation?.RolePermissions != null)
        {
            foreach (var rolePermission in utilisateur.IdRoleNavigation.RolePermissions)
            {
                if (rolePermission.Permission != null)
                {
                    claims.Add(new Claim("Permission", rolePermission.Permission.Nom));
                }
            }
        }

        return claims;
    }

    public async Task RefreshAuthenticationStateAsync()
    {
        try
        {
            var authState = await GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors du rafraîchissement de l'état d'authentification: {ex.Message}");
        }
    }
}

public class AuthenticationResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public Utilisateur? User { get; set; }
}
