using PNC.Models;

namespace PNC.Services;

public interface IEmpreinteService
{
    Task<List<Empreinte>> GetEmpreintesByPolicierIdAsync(string policierId);
    Task<Empreinte?> GetEmpreinteByIdAsync(string id);
    Task<Empreinte> CreateEmpreinteAsync(Empreinte empreinte);
    Task<Empreinte> UpdateEmpreinteAsync(Empreinte empreinte);
    Task<bool> DeleteEmpreinteAsync(string id);
    Task<string> SaveEmpreinteImageAsync(string base64Image, string policierId, string typeDoigt);
    Task<bool> DeleteEmpreinteImageAsync(string imagePath);
    Task<List<Empreinte>> GetEmpreintesManquantesAsync(string policierId);
    Task<bool> AllEmpreintesCapturedAsync(string policierId);
    string GenerateUniqueId();
}

