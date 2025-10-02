using PNC.Models;

namespace PNC.Services
{
    public interface IDocumentService
    {
        Task<Document?> GetDocumentByIdAsync(string id);
        Task<List<Document>> GetDocumentsByPolicierIdAsync(string policierId);
        Task<Document> CreateDocumentAsync(Document document);
        Task<Document> UpdateDocumentAsync(Document document);
        Task<bool> DeleteDocumentAsync(string id);
        Task<string> SaveDocumentImageAsync(string imageDataUrl, string policierId, string label, string? description = null, string? typeDocument = null);
        Task<bool> DeleteDocumentImageAsync(string imagePath);
        Task<List<Document>> GetDocumentsByTypeAsync(string policierId, string typeDocument);
        Task<int> GetDocumentCountByPolicierAsync(string policierId);
        Task<bool> DocumentExistsAsync(string policierId, string label);
    }
}
