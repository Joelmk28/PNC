using Microsoft.EntityFrameworkCore;
using PNC.Data;
using PNC.Models;

namespace PNC.Services;

public interface IPolicierStatisticsService
{
    // Statistiques générales
    Task<StatisticsSummary> GetStatisticsSummaryAsync();
    Task<int> GetTotalPoliciersAsync();
    Task<int> GetActivePoliciersAsync();
    Task<int> GetNewPoliciersThisMonthAsync();
    Task<int> GetPoliciersByGradeAsync(string grade);
    
    // Statistiques par unité
    Task<List<UniteStatistics>> GetUniteStatisticsAsync();
    Task<UniteStatistics> GetUniteStatisticsAsync(string uniteName);
    
    // Statistiques par grade
    Task<List<GradeStatistics>> GetGradeStatisticsAsync();
    Task<GradeStatistics> GetGradeStatisticsAsync(string grade);
    
    // Statistiques démographiques
    Task<DemographicStatistics> GetDemographicStatisticsAsync();
    Task<List<AgeGroupStatistics>> GetAgeGroupStatisticsAsync();
    Task<List<GenderStatistics>> GetGenderStatisticsAsync();
    
    // Statistiques de formation
    Task<List<FormationStatistics>> GetFormationStatisticsAsync();
    Task<List<LanguageStatistics>> GetLanguageStatisticsAsync();
    
    // Rapports
    Task<byte[]> GeneratePolicierReportAsync(string format = "pdf");
    Task<byte[]> GenerateUniteReportAsync(string uniteName, string format = "pdf");
    Task<byte[]> GenerateGradeReportAsync(string grade, string format = "pdf");
}

public class PolicierStatisticsService : IPolicierStatisticsService
{
    private readonly IDbContextFactory<BdPolicePncContext> _contextFactory;

    public PolicierStatisticsService(IDbContextFactory<BdPolicePncContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<StatisticsSummary> GetStatisticsSummaryAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        
        var total = await context.Policiers.CountAsync();
        var active = await context.Policiers.CountAsync(p => p.DateEntreePolice <= DateTime.Today);
        var thisMonth = await context.Policiers.CountAsync(p => 
            p.DateEntreePolice.Month == DateTime.Today.Month && 
            p.DateEntreePolice.Year == DateTime.Today.Year);
        
        var grades = await context.Policiers
            .GroupBy(p => p.NatureGrade)
            .Select(g => new { Grade = g.Key, Count = g.Count() })
            .ToListAsync();
        
        return new StatisticsSummary
        {
            TotalPoliciers = total,
            ActivePoliciers = active,
            NewThisMonth = thisMonth,
            GradeDistribution = grades.ToDictionary(g => g.Grade, g => g.Count)
        };
    }

    public async Task<int> GetTotalPoliciersAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Policiers.CountAsync();
    }

    public async Task<int> GetActivePoliciersAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Policiers.CountAsync(p => p.DateEntreePolice <= DateTime.Today);
    }

    public async Task<int> GetNewPoliciersThisMonthAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Policiers.CountAsync(p => 
            p.DateEntreePolice.Month == DateTime.Today.Month && 
            p.DateEntreePolice.Year == DateTime.Today.Year);
    }

    public async Task<int> GetPoliciersByGradeAsync(string grade)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Policiers.CountAsync(p => p.NatureGrade == grade);
    }

    public async Task<List<UniteStatistics>> GetUniteStatisticsAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Policiers
            .GroupBy(p => p.UniteAffectation)
            .Select(g => new UniteStatistics
            {
                UniteName = g.Key ?? "Non affecté",
                TotalPoliciers = g.Count(),
                ActivePoliciers = g.Count(p => p.DateEntreePolice <= DateTime.Today),
                NewThisMonth = g.Count(p => 
                    p.DateEntreePolice.Month == DateTime.Today.Month && 
                    p.DateEntreePolice.Year == DateTime.Today.Year)
            })
            .OrderByDescending(u => u.TotalPoliciers)
            .ToListAsync();
    }

    public async Task<UniteStatistics> GetUniteStatisticsAsync(string uniteName)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        
        var policiers = await context.Policiers
            .Where(p => p.UniteAffectation == uniteName || p.NomUnite == uniteName)
            .ToListAsync();
        
        return new UniteStatistics
        {
            UniteName = uniteName,
            TotalPoliciers = policiers.Count,
            ActivePoliciers = policiers.Count(p => p.DateEntreePolice <= DateTime.Today),
            NewThisMonth = policiers.Count(p => 
                p.DateEntreePolice.Month == DateTime.Today.Month && 
                p.DateEntreePolice.Year == DateTime.Today.Year)
        };
    }

    public async Task<List<GradeStatistics>> GetGradeStatisticsAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Policiers
            .GroupBy(p => p.NatureGrade)
            .Select(g => new GradeStatistics
            {
                Grade = g.Key ?? "Non défini",
                TotalPoliciers = g.Count(),
                ActivePoliciers = g.Count(p => p.DateEntreePolice <= DateTime.Today),
                NewThisMonth = g.Count(p => 
                    p.DateEntreePolice.Month == DateTime.Today.Month && 
                    p.DateEntreePolice.Year == DateTime.Today.Year),
                AverageAge = g.Average(p => DateTime.Today.Year - p.DateNaissance.Year)
            })
            .OrderByDescending(g => g.TotalPoliciers)
            .ToListAsync();
    }

    public async Task<GradeStatistics> GetGradeStatisticsAsync(string grade)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        
        var policiers = await context.Policiers
            .Where(p => p.NatureGrade == grade)
            .ToListAsync();
        
        return new GradeStatistics
        {
            Grade = grade,
            TotalPoliciers = policiers.Count,
            ActivePoliciers = policiers.Count(p => p.DateEntreePolice <= DateTime.Today),
            NewThisMonth = policiers.Count(p => 
                p.DateEntreePolice.Month == DateTime.Today.Month && 
                p.DateEntreePolice.Year == DateTime.Today.Year),
            AverageAge = policiers.Any() ? policiers.Average(p => DateTime.Today.Year - p.DateNaissance.Year) : 0
        };
    }

    public async Task<DemographicStatistics> GetDemographicStatisticsAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        
        var policiers = await context.Policiers.ToListAsync();
        
        var total = policiers.Count;
        if (total == 0)
            return new DemographicStatistics();
        
        var maleCount = policiers.Count(p => p.Sexe == "M");
        var femaleCount = policiers.Count(p => p.Sexe == "F");
        
        var ages = policiers.Select(p => DateTime.Today.Year - p.DateNaissance.Year).ToList();
        var averageAge = ages.Average();
        var minAge = ages.Min();
        var maxAge = ages.Max();
        
        return new DemographicStatistics
        {
            TotalPoliciers = total,
            MaleCount = maleCount,
            FemaleCount = femaleCount,
            MalePercentage = (double)maleCount / total * 100,
            FemalePercentage = (double)femaleCount / total * 100,
            AverageAge = averageAge,
            MinAge = minAge,
            MaxAge = maxAge
        };
    }

    public async Task<List<AgeGroupStatistics>> GetAgeGroupStatisticsAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        
        var policiers = await context.Policiers.ToListAsync();
        
        var ageGroups = new[]
        {
            new { Min = 18, Max = 25, Label = "18-25 ans" },
            new { Min = 26, Max = 35, Label = "26-35 ans" },
            new { Min = 36, Max = 45, Label = "36-45 ans" },
            new { Min = 46, Max = 55, Label = "46-55 ans" },
            new { Min = 56, Max = 65, Label = "56-65 ans" },
            new { Min = 66, Max = 70, Label = "66-70 ans" }
        };
        
        var result = new List<AgeGroupStatistics>();
        
        foreach (var group in ageGroups)
        {
            var count = policiers.Count(p => 
            {
                var age = DateTime.Today.Year - p.DateNaissance.Year;
                return age >= group.Min && age <= group.Max;
            });
            
            result.Add(new AgeGroupStatistics
            {
                AgeGroup = group.Label,
                Count = count,
                Percentage = policiers.Any() ? (double)count / policiers.Count * 100 : 0
            });
        }
        
        return result;
    }

    public async Task<List<GenderStatistics>> GetGenderStatisticsAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Policiers
            .GroupBy(p => p.Sexe)
            .Select(g => new GenderStatistics
            {
                Gender = g.Key ?? "Non défini",
                Count = g.Count(),
                Percentage = context.Policiers.Count() > 0 ? 
                    (double)g.Count() / context.Policiers.Count() * 100 : 0
            })
            .ToListAsync();
    }

    public async Task<List<FormationStatistics>> GetFormationStatisticsAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Formations
            .GroupBy(f => f.TypeFormation)
            .Select(g => new FormationStatistics
            {
                FormationType = g.Key ?? "Non défini",
                Count = g.Count(),
                UniquePoliciers = g.Select(f => f.IdPolicier).Distinct().Count()
            })
            .OrderByDescending(f => f.Count)
            .ToListAsync();
    }

    public async Task<List<LanguageStatistics>> GetLanguageStatisticsAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Langues
            .GroupBy(l => l.Libelle)
            .Select(g => new LanguageStatistics
            {
                Language = g.Key ?? "Non défini",
                Count = g.Count(),
                AverageWritingLevel = g.Average(l => l.NiveauEcriture),
                AverageReadingLevel = g.Average(l => l.NiveauLecture)
            })
            .OrderByDescending(l => l.Count)
            .ToListAsync();
    }

    // Implémentation des méthodes de génération de rapports (placeholder)
    public async Task<byte[]> GeneratePolicierReportAsync(string format = "pdf")
    {
        // TODO: Implémenter la génération de rapport
        await Task.Delay(100); // Simulation
        return new byte[0];
    }

    public async Task<byte[]> GenerateUniteReportAsync(string uniteName, string format = "pdf")
    {
        // TODO: Implémenter la génération de rapport par unité
        await Task.Delay(100); // Simulation
        return new byte[0];
    }

    public async Task<byte[]> GenerateGradeReportAsync(string grade, string format = "pdf")
    {
        // TODO: Implémenter la génération de rapport par grade
        await Task.Delay(100); // Simulation
        return new byte[0];
    }
}

#region Modèles de données pour les statistiques

public class StatisticsSummary
{
    public int TotalPoliciers { get; set; }
    public int ActivePoliciers { get; set; }
    public int NewThisMonth { get; set; }
    public Dictionary<string, int> GradeDistribution { get; set; } = new();
}

public class UniteStatistics
{
    public string UniteName { get; set; } = string.Empty;
    public int TotalPoliciers { get; set; }
    public int ActivePoliciers { get; set; }
    public int NewThisMonth { get; set; }
}

public class GradeStatistics
{
    public string Grade { get; set; } = string.Empty;
    public int TotalPoliciers { get; set; }
    public int ActivePoliciers { get; set; }
    public int NewThisMonth { get; set; }
    public double AverageAge { get; set; }
}

public class DemographicStatistics
{
    public int TotalPoliciers { get; set; }
    public int MaleCount { get; set; }
    public int FemaleCount { get; set; }
    public double MalePercentage { get; set; }
    public double FemalePercentage { get; set; }
    public double AverageAge { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
}

public class AgeGroupStatistics
{
    public string AgeGroup { get; set; } = string.Empty;
    public int Count { get; set; }
    public double Percentage { get; set; }
}

public class GenderStatistics
{
    public string Gender { get; set; } = string.Empty;
    public int Count { get; set; }
    public double Percentage { get; set; }
}

public class FormationStatistics
{
    public string FormationType { get; set; } = string.Empty;
    public int Count { get; set; }
    public int UniquePoliciers { get; set; }
}

public class LanguageStatistics
{
    public string Language { get; set; } = string.Empty;
    public int Count { get; set; }
    public double AverageWritingLevel { get; set; }
    public double AverageReadingLevel { get; set; }
}

#endregion
