using Microsoft.AspNetCore.Hosting;
using Microsoft.JSInterop;
using Microsoft.EntityFrameworkCore;
using PNC.Data;
using PNC.Models;
using System.Linq;
namespace PNC.Services;

public interface IPhotoService
{
    Task<string> SavePhotoAsync(string base64Image, string policierId, string type = "photo");
    Task<bool> DeletePhotoAsync(string imagePath);
    Task<List<string>> ListPhotosAsync();
}



   

public class PhotoService : IPhotoService
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<PhotoService> _logger;
     private readonly IDbContextFactory<BdPolicePncContext> _contextFactory;
    private readonly IPolicierValidationService _validationService;

    public PhotoService(IWebHostEnvironment env, ILogger<PhotoService> logger, IDbContextFactory<BdPolicePncContext> contextFactory, IPolicierValidationService validationService)
    {
        _env = env;
        _logger = logger;
        _contextFactory = contextFactory;
        _validationService = validationService;
    }
    

    public async Task<string> SavePhotoAsync(string base64Image, string policierId, string type = "photo")
    {
        try
        {
            _logger.LogInformation("Sauvegarde d'une image pour le policier {PolicierId}", policierId);

            // Récupérer le policier pour obtenir le NumeroNutp
            using var context = await _contextFactory.CreateDbContextAsync();
            var policier = await context.Policiers.FindAsync(policierId);
            if (policier == null)
            {
                throw new ArgumentException($"Policier avec l'ID {policierId} non trouvé");
            }

            // Supprimer "data:image/png;base64," si présent
            var base64Data = base64Image.Substring(base64Image.IndexOf(",") + 1);
            var bytes = Convert.FromBase64String(base64Data);

            // Dossier cible = wwwroot/servernas/{NumeroNutp}/photos
            var policierFolder = Path.Combine(_env.WebRootPath, "servernas", policier.NumeroNutp, "photos");
            if (!Directory.Exists(policierFolder))
            {
                Directory.CreateDirectory(policierFolder);
                _logger.LogInformation("📁 Dossier policier créé: {PolicierFolder}", policierFolder);
            }

            // Détecter le format de l'image et générer un nom unique avec timestamp
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var extension = base64Image.StartsWith("data:image/jpeg") ? "jpg" : "png";
            var fileName = $"{timestamp}_{type}_{policierId}_{Guid.NewGuid():N}.{extension}";
            var filePath = Path.Combine(policierFolder, fileName);

            // Sauvegarder le fichier
            await File.WriteAllBytesAsync(filePath, bytes);

            // Chemin relatif pour la base de données
            var relativePath = $"/servernas/{policier.NumeroNutp}/photos/{fileName}";
            
            // Mettre à jour le chemin de la photo dans la base de données
            policier.Photo = relativePath;
            await context.SaveChangesAsync();
            
            _logger.LogInformation("✅ Image sauvegardée avec succès: {FilePath}", relativePath);

            return relativePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erreur lors de la sauvegarde de l'image");
            throw;
        }
    }

    public async Task<bool> DeletePhotoAsync(string imagePath)
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
                _logger.LogInformation("🗑️ Image supprimée: {ImagePath}", imagePath);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erreur lors de la suppression de l'image {ImagePath}", imagePath);
            return false;
        }
    }

    public async Task<List<string>> ListPhotosAsync()
    {
        try
        {
            var servernasFolder = Path.Combine(_env.WebRootPath, "servernas");
            
            if (!Directory.Exists(servernasFolder))
                return new List<string>();

            var imageFiles = new List<string>();
            
            // Parcourir tous les dossiers de policiers
            var policierDirs = Directory.GetDirectories(servernasFolder);
            foreach (var policierDir in policierDirs)
            {
                var photosDir = Path.Combine(policierDir, "photos");
                if (Directory.Exists(photosDir))
                {
                    var photos = Directory.GetFiles(photosDir, "*.png")
                        .Concat(Directory.GetFiles(photosDir, "*.jpg"))
                        .Select(file => $"/servernas/{Path.GetFileName(policierDir)}/photos/{Path.GetFileName(file)}")
                        .ToList();
                    
                    imageFiles.AddRange(photos);
                }
            }

            return imageFiles.OrderByDescending(path => path).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erreur lors de la récupération de la liste des images");
            return new List<string>();
        }
    }
}
