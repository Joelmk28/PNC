using Microsoft.EntityFrameworkCore;
using PNC.Data;
using PNC.Models;
using System.Text.RegularExpressions;

namespace PNC.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDbContextFactory<BdPolicePncContext> _contextFactory;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<DocumentService> _logger;

        public DocumentService(
            IDbContextFactory<BdPolicePncContext> contextFactory,
            IWebHostEnvironment environment,
            ILogger<DocumentService> logger)
        {
            _contextFactory = contextFactory;
            _environment = environment;
            _logger = logger;
        }

        public async Task<Document?> GetDocumentByIdAsync(string id)
        {
            try
            {
                using var context = await _contextFactory.CreateDbContextAsync();
                return await context.Documents
                    .Include(d => d.IdPolicierNavigation)
                    .FirstOrDefaultAsync(d => d.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération du document {DocumentId}", id);
                return null;
            }
        }

        public async Task<List<Document>> GetDocumentsByPolicierIdAsync(string policierId)
        {
            try
            {
                using var context = await _contextFactory.CreateDbContextAsync();
                return await context.Documents
                    .Where(d => d.IdPolicier == policierId && d.EstActif)
                    .OrderByDescending(d => d.DateCreation)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des documents pour le policier {PolicierId}", policierId);
                return new List<Document>();
            }
        }

        public async Task<Document> CreateDocumentAsync(Document document)
        {
            try
            {
                using var context = await _contextFactory.CreateDbContextAsync();
                context.Documents.Add(document);
                await context.SaveChangesAsync();
                
                _logger.LogInformation("Document créé avec succès: {DocumentId}", document.Id);
                return document;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création du document");
                throw;
            }
        }

        public async Task<Document> UpdateDocumentAsync(Document document)
        {
            try
            {
                using var context = await _contextFactory.CreateDbContextAsync();
                document.DateModification = DateTime.Now;
                context.Documents.Update(document);
                await context.SaveChangesAsync();
                
                _logger.LogInformation("Document mis à jour avec succès: {DocumentId}", document.Id);
                return document;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour du document {DocumentId}", document.Id);
                throw;
            }
        }

        public async Task<bool> DeleteDocumentAsync(string id)
        {
            try
            {
                using var context = await _contextFactory.CreateDbContextAsync();
                var document = await context.Documents.FindAsync(id);
                if (document != null)
                {
                    // Suppression logique
                    document.EstActif = false;
                    document.Status = DocumentStatus.Supprime;
                    document.DateModification = DateTime.Now;
                    await context.SaveChangesAsync();
                    
                    _logger.LogInformation("Document supprimé avec succès: {DocumentId}", id);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression du document {DocumentId}", id);
                return false;
            }
        }

        public async Task<string> SaveDocumentImageAsync(string imageDataUrl, string policierId, string label, string? description = null, string? typeDocument = null)
        {
            try
            {
                // Récupérer le policier pour obtenir le NumeroNutp
                using var context = await _contextFactory.CreateDbContextAsync();
                var policier = await context.Policiers.FindAsync(policierId);
                if (policier == null)
                {
                    throw new ArgumentException($"Policier avec l'ID {policierId} non trouvé");
                }

                // Créer le dossier pour ce policier en utilisant NumeroNutp
                var policierFolder = Path.Combine(_environment.WebRootPath, "servernas", policier.NumeroNutp, "documents");
                if (!Directory.Exists(policierFolder))
                {
                    Directory.CreateDirectory(policierFolder);
                }

                // Générer un nom de fichier unique
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var sanitizedLabel = SanitizeFileName(label);
                var fileName = $"{timestamp}_{sanitizedLabel}_{Guid.NewGuid():N}.jpg";
                var fullPath = Path.Combine(policierFolder, fileName);

                // Convertir la data URL en bytes
                string base64Data = imageDataUrl.Replace("data:image/jpeg;base64,", "").Replace("data:image/png;base64,", "");
                byte[] imageBytes = Convert.FromBase64String(base64Data);
                
                // Sauvegarder le fichier
                await File.WriteAllBytesAsync(fullPath, imageBytes);

                // Créer l'enregistrement en base de données
                var document = new Document
                {
                    Id = GenerateShortId(),
                    IdPolicier = policierId,
                    UrlDocument = $"/servernas/{policier.NumeroNutp}/documents/{fileName}",
                    Label = label,
                    Description = description,
                    TypeDocument = typeDocument ?? "Photo",
                    Extension = "jpg",
                    TailleFichier = imageBytes.Length,
                    Status = DocumentStatus.PhotoCapturee,
                    EstActif = true,
                    DateCreation = DateTime.Now
                };

                await CreateDocumentAsync(document);

                _logger.LogInformation("Image de document sauvegardée avec succès: {FilePath}", document.UrlDocument);
                return document.UrlDocument;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la sauvegarde de l'image de document");
                throw;
            }
        }

        public async Task<bool> DeleteDocumentImageAsync(string imagePath)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath))
                    return false;

                var absolutePath = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/'));
                
                if (File.Exists(absolutePath))
                {
                    File.Delete(absolutePath);
                    _logger.LogInformation("Image de document supprimée: {ImagePath}", imagePath);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de l'image de document {ImagePath}", imagePath);
                return false;
            }
        }

        public async Task<List<Document>> GetDocumentsByTypeAsync(string policierId, string typeDocument)
        {
            try
            {
                using var context = await _contextFactory.CreateDbContextAsync();
                return await context.Documents
                    .Where(d => d.IdPolicier == policierId && d.TypeDocument == typeDocument && d.EstActif)
                    .OrderByDescending(d => d.DateCreation)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des documents par type {TypeDocument} pour le policier {PolicierId}", typeDocument, policierId);
                return new List<Document>();
            }
        }

        public async Task<int> GetDocumentCountByPolicierAsync(string policierId)
        {
            try
            {
                using var context = await _contextFactory.CreateDbContextAsync();
                return await context.Documents
                    .CountAsync(d => d.IdPolicier == policierId && d.EstActif);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du comptage des documents pour le policier {PolicierId}", policierId);
                return 0;
            }
        }

        public async Task<bool> DocumentExistsAsync(string policierId, string label)
        {
            try
            {
                using var context = await _contextFactory.CreateDbContextAsync();
                return await context.Documents
                    .AnyAsync(d => d.IdPolicier == policierId && d.Label == label && d.EstActif);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la vérification de l'existence du document {Label} pour le policier {PolicierId}", label, policierId);
                return false;
            }
        }

        private string GenerateShortId()
        {
            return Guid.NewGuid().ToString("N")[..10];
        }

        private string SanitizeFileName(string fileName)
        {
            // Remplacer les caractères non autorisés dans les noms de fichiers
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
