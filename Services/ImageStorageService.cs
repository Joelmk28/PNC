using System.IO;
using System.Text.RegularExpressions;

namespace PNC.Services
{
    public interface IImageStorageService
    {
        Task<string> SavePolicierImageAsync(string imageDataUrl, string policierId, string policierName);
        Task<bool> DeletePolicierImageAsync(string imagePath);
        string GetImageUrl(string imagePath);
    }

    public class ImageStorageService : IImageStorageService
    {
        private readonly string _baseImagePath;
        private readonly string _webRootPath;
        private readonly IWebHostEnvironment _environment;

        public ImageStorageService(IWebHostEnvironment environment)
        {
            _environment = environment;
            _webRootPath = _environment.WebRootPath;
            _baseImagePath = Path.Combine(_webRootPath, "servernas");
        }

        public async Task<string> SavePolicierImageAsync(string imageDataUrl, string policierId, string policierName)
        {
            try
            {
                // Créer le nom du dossier pour ce policier
                string sanitizedName = SanitizeFileName(policierName);
                string policierFolder = Path.Combine(_baseImagePath, $"{sanitizedName}_{policierId}", "images");
                
                // Créer le dossier s'il n'existe pas
                if (!Directory.Exists(policierFolder))
                {
                    Directory.CreateDirectory(policierFolder);
                }

                // Générer un nom de fichier unique avec timestamp
                string fileName = $"photo_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
                string fullPath = Path.Combine(policierFolder, fileName);

                // Convertir la data URL en bytes et sauvegarder
                string base64Data = imageDataUrl.Replace("data:image/png;base64,", "").Replace("data:image/jpeg;base64,", "");
                byte[] imageBytes = Convert.FromBase64String(base64Data);
                
                await File.WriteAllBytesAsync(fullPath, imageBytes);

                // Retourner le chemin relatif pour stocker en base
                string relativePath = Path.Combine("servernas", $"{sanitizedName}_{policierId}", "images", fileName);
                return relativePath.Replace("\\", "/"); // Normaliser pour le web
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la sauvegarde de l'image: {ex.Message}");
                throw new InvalidOperationException($"Impossible de sauvegarder l'image: {ex.Message}", ex);
            }
        }

        public Task<bool> DeletePolicierImageAsync(string imagePath)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath))
                    return Task.FromResult(true);

                string fullPath = Path.Combine(_webRootPath, imagePath);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return Task.FromResult(true);
                }
                return Task.FromResult(true); // Si le fichier n'existe pas, on considère que c'est OK
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la suppression de l'image: {ex.Message}");
                return Task.FromResult(false);
            }
        }

        public string GetImageUrl(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return string.Empty;

            // Retourner l'URL relative pour l'affichage
            return $"/{imagePath.Replace("\\", "/")}";
        }

        private string SanitizeFileName(string fileName)
        {
            // Remplacer les caractères non autorisés dans les noms de dossiers
            string sanitized = Regex.Replace(fileName, @"[<>:""/\\|?*]", "_");
            sanitized = Regex.Replace(sanitized, @"\s+", "_"); // Remplacer les espaces par des underscores
            sanitized = sanitized.Trim('_'); // Enlever les underscores en début/fin
            
            // Limiter la longueur
            if (sanitized.Length > 50)
                sanitized = sanitized.Substring(0, 50);
                
            return sanitized;
        }
    }
}
