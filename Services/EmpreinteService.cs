using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using PNC.Data;
using PNC.Models;

namespace PNC.Services;

public class EmpreinteService : IEmpreinteService
{
    private readonly IDbContextFactory<BdPolicePncContext> _dbFactory;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<EmpreinteService> _logger;

    public EmpreinteService(IDbContextFactory<BdPolicePncContext> dbFactory, IWebHostEnvironment env, ILogger<EmpreinteService> logger)
    {
        _dbFactory = dbFactory;
        _env = env;
        _logger = logger;
    }

    public async Task<List<Empreinte>> GetEmpreintesByPolicierIdAsync(string policierId)
    {
        using var context = _dbFactory.CreateDbContext();
        return await context.Empreintes
            .Where(e => e.IdPolicier == policierId)
            .OrderBy(e => e.TypeDoigt)
            .ToListAsync();
    }

    public async Task<Empreinte?> GetEmpreinteByIdAsync(string id)
    {
        using var context = _dbFactory.CreateDbContext();
        return await context.Empreintes
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Empreinte> CreateEmpreinteAsync(Empreinte empreinte)
    {
        using var context = _dbFactory.CreateDbContext();
        context.Empreintes.Add(empreinte);
        await context.SaveChangesAsync();
        return empreinte;
    }

    public async Task<Empreinte> UpdateEmpreinteAsync(Empreinte empreinte)
    {
        using var context = _dbFactory.CreateDbContext();
        context.Empreintes.Update(empreinte);
        await context.SaveChangesAsync();
        return empreinte;
    }

    public async Task<bool> DeleteEmpreinteAsync(string id)
    {
        using var context = _dbFactory.CreateDbContext();
        var empreinte = await context.Empreintes.FindAsync(id);
        if (empreinte == null) return false;

        context.Empreintes.Remove(empreinte);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SaveEmpreinteImageAsync(string empreinteId, string imageUrl)
    {
        using var context = _dbFactory.CreateDbContext();
        var empreinte = await context.Empreintes.FindAsync(empreinteId);
        if (empreinte == null) return false;

        empreinte.Urlepreinte = imageUrl;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Empreinte>> GetEmpreintesManquantesAsync(string policierId)
    {
        using var context = _dbFactory.CreateDbContext();
        var empreintesExistantes = await context.Empreintes
            .Where(e => e.IdPolicier == policierId)
            .Select(e => e.TypeDoigt)
            .ToListAsync();

        var tousLesDoigts = new List<string>
        {
            "4 Doigts Droits",
            "4 Doigts Gauches", 
            "2 Pouces"
        };

        var doigtsManquants = tousLesDoigts.Except(empreintesExistantes).ToList();

        return doigtsManquants.Select(doigt => new Empreinte
        {
            Id = GenerateShortId(),
            IdPolicier = policierId,
            TypeDoigt = doigt,
            Urlepreinte = string.Empty
        }).ToList();
    }

    public async Task<bool> AllEmpreintesCapturedAsync(string policierId)
    {
        using var context = _dbFactory.CreateDbContext();
        var count = await context.Empreintes
            .Where(e => e.IdPolicier == policierId && !string.IsNullOrEmpty(e.Urlepreinte))
            .CountAsync();
        return count >= 3; // 3 images au total
    }

    private string GenerateShortId()
    {
        return Guid.NewGuid().ToString("N")[..10];
    }

    public string GenerateUniqueId()
    {
        return Guid.NewGuid().ToString("N")[..10];
    }

    public async Task<string> SaveEmpreinteImageAsync(string base64Image, string policierId, string typeDoigt)
    {
        try
        {
            _logger.LogInformation("Sauvegarde d'une empreinte pour le policier {PolicierId}, type: {TypeDoigt}", policierId, typeDoigt);

            // Supprimer "data:image/png;base64," si présent
            var base64Data = base64Image.Substring(base64Image.IndexOf(",") + 1);
            var bytes = Convert.FromBase64String(base64Data);

            // Récupérer le NumeroNutp du policier
            using var context = _dbFactory.CreateDbContext();
            var policier = await context.Policiers.FindAsync(policierId);
            if (policier == null)
            {
                throw new ArgumentException($"Policier avec l'ID {policierId} non trouvé");
            }

            // Créer le dossier spécifique au policier
            var policierFolder = Path.Combine(_env.WebRootPath, "servernas", policier.NumeroNutp, "empreintes");
            if (!Directory.Exists(policierFolder))
            {
                Directory.CreateDirectory(policierFolder);
                _logger.LogInformation("Dossier empreintes créé: {PolicierFolder}", policierFolder);
            }

            // Générer un nom de fichier unique
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var extension = base64Image.StartsWith("data:image/jpeg") ? "jpg" : 
                           base64Image.StartsWith("data:image/png") ? "png" : 
                           base64Image.StartsWith("data:image/bmp") ? "bmp" : "png";
            
            var fileName = $"{timestamp}_{typeDoigt.Replace(" ", "_")}_{Guid.NewGuid():N}.{extension}";
            var filePath = Path.Combine(policierFolder, fileName);

            // Sauvegarder le fichier
            await File.WriteAllBytesAsync(filePath, bytes);

            var relativePath = $"/servernas/{policier.NumeroNutp}/empreintes/{fileName}";
            _logger.LogInformation("✅ Empreinte sauvegardée avec succès: {FilePath}", relativePath);

            return relativePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erreur lors de la sauvegarde de l'empreinte");
            throw;
        }
    }

    public async Task<bool> DeleteEmpreinteImageAsync(string imagePath)
    {
        try
        {
            if (string.IsNullOrEmpty(imagePath))
                return false;

            // Convertir le chemin relatif en chemin absolu
            var absolutePath = Path.Combine(_env.WebRootPath, imagePath.TrimStart('/'));
            
            if (File.Exists(absolutePath))
            {
                File.Delete(absolutePath);
                _logger.LogInformation("🗑️ Empreinte supprimée: {ImagePath}", imagePath);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erreur lors de la suppression de l'empreinte {ImagePath}", imagePath);
            return false;
        }
    }
}

