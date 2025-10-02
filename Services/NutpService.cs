using Microsoft.EntityFrameworkCore;
using PNC.Data;
using PNC.Models;

namespace PNC.Services;

public class NutpService : INutpService
{
    private readonly IDbContextFactory<BdPolicePncContext> _contextFactory;

    public NutpService(IDbContextFactory<BdPolicePncContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Nutp?> GetNutpByNumeroAsync(string numeroNutp)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Nutps
            .FirstOrDefaultAsync(n => n.NumeroNutp == numeroNutp);
    }

    public async Task<bool> IsNutpAvailableAsync(string numeroNutp)
    {
        var nutp = await GetNutpByNumeroAsync(numeroNutp);
        
        if (nutp == null)
            return false; // NUTP n'existe pas dans le système
        
        var status = nutp.Status?.ToUpper();
        return status == "FREE" || string.IsNullOrEmpty(nutp.Status);
    }

    public async Task<bool> MarkNutpAsUsedAsync(string numeroNutp)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var nutp = await context.Nutps
            .FirstOrDefaultAsync(n => n.NumeroNutp == numeroNutp);
        
        if (nutp == null)
            return false;
        
        nutp.Status = "BUSY";
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> MarkNutpAsAvailableAsync(string numeroNutp)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var nutp = await context.Nutps
            .FirstOrDefaultAsync(n => n.NumeroNutp == numeroNutp);
        
        if (nutp == null)
            return false;
        
        nutp.Status = "FREE";
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Nutp>> GetAllNutpsAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Nutps
            .OrderBy(n => n.NumeroNutp)
            .ToListAsync();
    }

    public async Task<Nutp> CreateNutpAsync(string numeroNutp, string status = "FREE")
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        
        var nutp = new Nutp
        {
            Id = Guid.NewGuid().ToString("N")[..10], // Génère un ID de 10 caractères
            NumeroNutp = numeroNutp,
            Status = status
        };
        
        context.Nutps.Add(nutp);
        await context.SaveChangesAsync();
        
        return nutp;
    }
}
