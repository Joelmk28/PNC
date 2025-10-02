using PNC.Models;

namespace PNC.Services;

public interface INutpService
{
    Task<Nutp?> GetNutpByNumeroAsync(string numeroNutp);
    Task<bool> IsNutpAvailableAsync(string numeroNutp);
    Task<bool> MarkNutpAsUsedAsync(string numeroNutp);
    Task<bool> MarkNutpAsAvailableAsync(string numeroNutp);
    Task<List<Nutp>> GetAllNutpsAsync();
    Task<Nutp> CreateNutpAsync(string numeroNutp, string status = "disponible");
}
