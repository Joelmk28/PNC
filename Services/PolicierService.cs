using Microsoft.EntityFrameworkCore;
using PNC.Data;
using PNC.Models;
using System.Linq;

namespace PNC.Services;

public interface IPolicierService
{
    // Opérations de lecture
    Task<List<Policier>> GetAllPoliciersAsync();
    Task<Policier?> GetPolicierByIdAsync(string id);
    Task<Policier?> GetPolicierByMatriculeAsync(string matricule);
    Task<Policier?> GetPolicierByNutpAsync(string numeroNutp);
Policier? GetPolicierByNutpSimpleAsync(string numeroNutp);
    Task<List<Policier>> SearchPoliciersAsync(string searchTerm);
    Task<List<Policier>> GetPoliciersByGradeAsync(string grade);
    Task<List<Policier>> GetPoliciersByUniteAsync(string unite);
    Task<int> GetTotalPoliciersCountAsync();
    Task<List<Policier>> GetPoliciersPaginatedAsync(int page, int pageSize);
    
    // Opérations d'écriture
    Task<Policier?> CreatePolicierAsync(Policier policier);
    Task<Policier?> UpdatePolicierAsync(Policier policier);
    Task<bool> DeletePolicierAsync(string id);
    
    // Opérations sur les collections
    Task<bool> AddConjointToPolicierAsync(string policierId, Conjoint conjoint);
    Task<bool> RemoveConjointFromPolicierAsync(string policierId, string conjointId);
    Task<bool> UpdateConjointAsync(Conjoint conjoint);
    
    Task<bool> AddEnfantToPolicierAsync(string policierId, Enfant enfant);
    Task<bool> RemoveEnfantFromPolicierAsync(string policierId, string enfantId);
    Task<bool> UpdateEnfantAsync(Enfant enfant);
    
    Task<bool> AddFormationToPolicierAsync(string policierId, Formation formation);
    Task<bool> RemoveFormationFromPolicierAsync(string policierId, string formationId);
    Task<bool> UpdateFormationAsync(Formation formation);
    
    Task<bool> AddLangueToPolicierAsync(string policierId, Langue langue);
    Task<bool> RemoveLangueFromPolicierAsync(string policierId, string langueId);
    Task<bool> UpdateLangueAsync(Langue langue);
    
    Task<bool> AddSportToPolicierAsync(string policierId, Sport sport);
    Task<bool> RemoveSportFromPolicierAsync(string policierId, string sportId);
    Task<bool> UpdateSportAsync(Sport sport);
    
    Task<bool> AddDistinctionToPolicierAsync(string policierId, DistinctionHonorifique distinction);
    Task<bool> RemoveDistinctionFromPolicierAsync(string policierId, string distinctionId);
    Task<bool> UpdateDistinctionAsync(DistinctionHonorifique distinction);
    
    Task<bool> AddHistAffectationToPolicierAsync(string policierId, HistAffectation affectation);
    Task<bool> RemoveHistAffectationFromPolicierAsync(string policierId, string affectationId);
    Task<bool> UpdateHistAffectationAsync(HistAffectation affectation);
    
    Task<bool> AddPersonnePrevenirToPolicierAsync(string policierId, PersonnePrevenir personne);
    Task<bool> RemovePersonnePrevenirFromPolicierAsync(string policierId, string personneId);
    Task<bool> UpdatePersonnePrevenirAsync(PersonnePrevenir personne);
    
    // Méthodes utilitaires
    string GenerateUniqueId();
    
    Task<bool> AddEmpreinteToPolicierAsync(string policierId, Empreinte empreinte);
    Task<bool> RemoveEmpreinteFromPolicierAsync(string policierId, string empreinteId);
    Task<bool> UpdateEmpreinteAsync(Empreinte empreinte);
    
    // Opérations de validation et vérification
    Task<bool> IsMatriculeUniqueAsync(string matricule, string? excludeId = null);
    Task<bool> IsNutpUniqueAsync(string numeroNutp, string? excludeId = null);
    Task<bool> PolicierExistsAsync(string id);
    
    // Opérations de sauvegarde
    Task<bool> SaveChangesAsync();
    Task<bool> ValidateAndSavePolicierAsync(Policier policier);
}

public class PolicierService : IPolicierService
{
    private readonly IDbContextFactory<BdPolicePncContext> _contextFactory;
    private readonly IPolicierValidationService _validationService;

    public PolicierService(
        IDbContextFactory<BdPolicePncContext> contextFactory,
        IPolicierValidationService validationService)
    {
        _contextFactory = contextFactory;
        _validationService = validationService;
    }

    #region Opérations de lecture

    public async Task<List<Policier>> GetAllPoliciersAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Policiers
            .Include(p => p.Conjoints)
            .Include(p => p.Enfants)
            .Include(p => p.Formations)
            .Include(p => p.Langues)
            .Include(p => p.Sports)
            .Include(p => p.DistinctionHonorifiques)
            .Include(p => p.HistAffectations)
            .Include(p => p.PersonnePrevenirs)
            .Include(p => p.Empreintes)
            .OrderBy(p => p.Nom)
            .ThenBy(p => p.Prenom)
            .ToListAsync();
    }

    public async Task<Policier?> GetPolicierByIdAsync(string id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Policiers
            .Include(p => p.Conjoints)
            .Include(p => p.Enfants)
            .Include(p => p.Formations)
            .Include(p => p.Langues)
            .Include(p => p.Sports)
            .Include(p => p.DistinctionHonorifiques)
            .Include(p => p.HistAffectations)
            .Include(p => p.PersonnePrevenirs)
            .Include(p => p.Empreintes)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Policier?> GetPolicierByMatriculeAsync(string matricule)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Policiers
            .Include(p => p.Conjoints)
            .Include(p => p.Enfants)
            .Include(p => p.Formations)
            .Include(p => p.Langues)
            .Include(p => p.Sports)
            .Include(p => p.DistinctionHonorifiques)
            .Include(p => p.HistAffectations)
            .Include(p => p.PersonnePrevenirs)
            .Include(p => p.Empreintes)
            .FirstOrDefaultAsync(p => p.Matricule == matricule);
    }

    public async Task<Policier?> GetPolicierByNutpAsync(string numeroNutp)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Policiers
            .Include(p => p.Conjoints)
            .Include(p => p.Enfants)
            .Include(p => p.Formations)
            .Include(p => p.Langues)
            .Include(p => p.Sports)
            .Include(p => p.DistinctionHonorifiques)
            .Include(p => p.HistAffectations)
            .Include(p => p.PersonnePrevenirs)
            .Include(p => p.Empreintes)
            .FirstOrDefaultAsync(p => p.NumeroNutp == numeroNutp);
    }


    public Policier? GetPolicierByNutpSimpleAsync(string numeroNutp)
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Policiers.FirstOrDefault(p => p.NumeroNutp == numeroNutp);
    }

    public async Task<List<Policier>> SearchPoliciersAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return new List<Policier>();

        using var context = await _contextFactory.CreateDbContextAsync();
        var term = searchTerm.ToLower();
        
        return await context.Policiers
            .Include(p => p.Conjoints)
            .Include(p => p.Enfants)
            .Include(p => p.Formations)
            .Include(p => p.Langues)
            .Include(p => p.Sports)
            .Include(p => p.DistinctionHonorifiques)
            .Include(p => p.HistAffectations)
            .Include(p => p.PersonnePrevenirs)
            .Include(p => p.Empreintes)
            .Where(p => p.Nom.ToLower().Contains(term) ||
                       p.Prenom.ToLower().Contains(term) ||
                       p.Matricule.ToLower().Contains(term) ||
                       p.NumeroNutp.ToLower().Contains(term) ||
                       p.NatureGrade.ToLower().Contains(term))
            .OrderBy(p => p.Nom)
            .ThenBy(p => p.Prenom)
            .ToListAsync();
    }

    public async Task<List<Policier>> GetPoliciersByGradeAsync(string grade)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Policiers
            .Include(p => p.Conjoints)
            .Include(p => p.Enfants)
            .Include(p => p.Formations)
            .Include(p => p.Langues)
            .Include(p => p.Sports)
            .Include(p => p.DistinctionHonorifiques)
            .Include(p => p.HistAffectations)
            .Include(p => p.PersonnePrevenirs)
            .Include(p => p.Empreintes)
            .Where(p => p.NatureGrade == grade)
            .OrderBy(p => p.Nom)
            .ThenBy(p => p.Prenom)
            .ToListAsync();
    }

    public async Task<List<Policier>> GetPoliciersByUniteAsync(string unite)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Policiers
            .Include(p => p.Conjoints)
            .Include(p => p.Enfants)
            .Include(p => p.Formations)
            .Include(p => p.Langues)
            .Include(p => p.Sports)
            .Include(p => p.DistinctionHonorifiques)
            .Include(p => p.HistAffectations)
            .Include(p => p.PersonnePrevenirs)
            .Include(p => p.Empreintes)
            .Where(p => p.UniteAffectation == unite || p.NomUnite == unite)
            .OrderBy(p => p.Nom)
            .ThenBy(p => p.Prenom)
            .ToListAsync();
    }

    public async Task<int> GetTotalPoliciersCountAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Policiers.CountAsync();
    }

    public async Task<List<Policier>> GetPoliciersPaginatedAsync(int page, int pageSize)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Policiers
            .Include(p => p.Conjoints)
            .Include(p => p.Enfants)
            .Include(p => p.Formations)
            .Include(p => p.Langues)
            .Include(p => p.Sports)
            .Include(p => p.DistinctionHonorifiques)
            .Include(p => p.HistAffectations)
            .Include(p => p.PersonnePrevenirs)
            .Include(p => p.Empreintes)
            .OrderBy(p => p.Nom)
            .ThenBy(p => p.Prenom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    #endregion

    #region Opérations d'écriture

    public async Task<Policier?> CreatePolicierAsync(Policier policier)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            // Générer un ID unique si pas déjà défini
            if (string.IsNullOrEmpty(policier.Id))
                policier.Id = GenerateShortId();
            
            // Sauvegarder temporairement les collections
            var conjoints = policier.Conjoints ?? new List<Conjoint>();
            var enfants = policier.Enfants ?? new List<Enfant>();
            var formations = policier.Formations ?? new List<Formation>();
            var langues = policier.Langues ?? new List<Langue>();
            var sports = policier.Sports ?? new List<Sport>();
            var distinctions = policier.DistinctionHonorifiques ?? new List<DistinctionHonorifique>();
            var affectations = policier.HistAffectations ?? new List<HistAffectation>();
            var fonctions = policier.HistFonctions ?? new List<HistFonction>();
            var grades = policier.HistGrades ?? new List<HistGrade>();
            var personnes = policier.PersonnePrevenirs ?? new List<PersonnePrevenir>();
            var empreintes = policier.Empreintes ?? new List<Empreinte>();
            var fris = policier.Fris ?? new List<Fri>();
            
            // Vider temporairement les collections pour éviter les erreurs de contrainte
            policier.Conjoints = new List<Conjoint>();
            policier.Enfants = new List<Enfant>();
            policier.Formations = new List<Formation>();
            policier.Langues = new List<Langue>();
            policier.Sports = new List<Sport>();
            policier.DistinctionHonorifiques = new List<DistinctionHonorifique>();
            policier.HistAffectations = new List<HistAffectation>();
            policier.HistFonctions = new List<HistFonction>();
            policier.HistGrades = new List<HistGrade>();
            policier.PersonnePrevenirs = new List<PersonnePrevenir>();
            policier.Empreintes = new List<Empreinte>();
            policier.Fris = new List<Fri>();
            
            // Créer automatiquement un FRI initial pour le nouveau policier
            await CreateInitialFriAsync(context, policier);

            var freeNUMP = await context.Nutps.FirstOrDefaultAsync(n => n.NumeroNutp == policier.NumeroNutp);
            if (freeNUMP == null)
            {
                return null;
            }
            freeNUMP.Status = "BUSY";
            context.Nutps.Update(freeNUMP);
            policier.NumeroNutp = freeNUMP.NumeroNutp;
            
            // Insérer d'abord le policier principal
            context.Policiers.Add(policier);
            await context.SaveChangesAsync();
            
            // Maintenant ajouter les collections une par une
            foreach (var conjoint in conjoints)
            {
                if (string.IsNullOrEmpty(conjoint.Id))
                    conjoint.Id = GenerateShortId();
                conjoint.IdPolicier = policier.Id;
                context.Conjoints.Add(conjoint);
            }
            
            foreach (var enfant in enfants)
            {
                if (string.IsNullOrEmpty(enfant.Id))
                    enfant.Id = GenerateShortId();
                enfant.IdPolicier = policier.Id;
                context.Enfants.Add(enfant);
            }
            
            foreach (var formation in formations)
            {
                if (string.IsNullOrEmpty(formation.Id))
                    formation.Id = GenerateShortId();
                formation.IdPolicier = policier.Id;
                context.Formations.Add(formation);
            }
            
            foreach (var langue in langues)
            {
                if (string.IsNullOrEmpty(langue.Id))
                    langue.Id = GenerateShortId();
                langue.IdPolicier = policier.Id;
                context.Langues.Add(langue);
            }
            
            foreach (var sport in sports)
            {
                if (string.IsNullOrEmpty(sport.Id))
                    sport.Id = GenerateShortId();
                sport.IdPolicier = policier.Id;
                context.Sports.Add(sport);
            }
            
            foreach (var distinction in distinctions)
            {
                if (string.IsNullOrEmpty(distinction.Id))
                    distinction.Id = GenerateShortId();
                distinction.IdPolicier = policier.Id;
                context.DistinctionHonorifiques.Add(distinction);
            }
            
            foreach (var affectation in affectations)
            {
                if (string.IsNullOrEmpty(affectation.Id))
                    affectation.Id = GenerateShortId();
                affectation.IdPolicier = policier.Id;
                context.HistAffectations.Add(affectation);
            }
            
            foreach (var fonction in fonctions)
            {
                if (string.IsNullOrEmpty(fonction.Id))
                    fonction.Id = GenerateShortId();
                fonction.IdPolicier = policier.Id;
                context.HistFonctions.Add(fonction);
            }
            
            foreach (var grade in grades)
            {
                if (string.IsNullOrEmpty(grade.Id))
                    grade.Id = GenerateShortId();
                grade.IdPolicier = policier.Id;
                context.HistGrades.Add(grade);
            }
            
            foreach (var personne in personnes)
            {
                if (string.IsNullOrEmpty(personne.Id))
                    personne.Id = GenerateShortId();
                personne.IdPolicier = policier.Id;
                context.PersonnePrevenirs.Add(personne);
            }
            
            foreach (var empreinte in empreintes)
            {
                empreinte.IdPolicier = policier.Id;
                context.Empreintes.Add(empreinte);
            }
            
            foreach (var fri in fris)
            {
                fri.IdPolicier = policier.Id;
                context.Fris.Add(fri);
            }
            
            // Sauvegarder toutes les collections
            await context.SaveChangesAsync();
            
            // Restaurer les collections dans l'objet policier retourné
            policier.Conjoints = conjoints;
            policier.Enfants = enfants;
            policier.Formations = formations;
            policier.Langues = langues;
            policier.Sports = sports;
            policier.DistinctionHonorifiques = distinctions;
            policier.HistAffectations = affectations;
            policier.HistFonctions = fonctions;
            policier.HistGrades = grades;
            policier.PersonnePrevenirs = personnes;
            policier.Empreintes = empreintes;
            policier.Fris = fris;
            
            return policier;
        }
        catch (Exception ex)
        {
            // Log l'erreur ici
            Console.WriteLine($"Erreur lors de la création du policier: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return null;
        }
    }

    public async Task<Policier?> UpdatePolicierAsync(Policier policier)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var existingPolicier = await context.Policiers
                .Include(p => p.Conjoints)
                .Include(p => p.Enfants)
                .Include(p => p.Formations)
                .Include(p => p.Langues)
                .Include(p => p.Sports)
                .Include(p => p.DistinctionHonorifiques)
                .Include(p => p.HistAffectations)
                .Include(p => p.HistFonctions)
                .Include(p => p.HistGrades)
                .Include(p => p.PersonnePrevenirs)
                .Include(p => p.Empreintes)
                .Include(p => p.Fris)
                .FirstOrDefaultAsync(p => p.Id == policier.Id);
            
            if (existingPolicier == null)
                return null;
            
            // Mettre à jour les propriétés principales
            context.Entry(existingPolicier).CurrentValues.SetValues(policier);
            
            // Mettre à jour les collections
            UpdateCollection(context, existingPolicier.Conjoints, policier.Conjoints);
            UpdateCollection(context, existingPolicier.Enfants, policier.Enfants);
            UpdateCollection(context, existingPolicier.Formations, policier.Formations);
            UpdateCollection(context, existingPolicier.Langues, policier.Langues);
            UpdateCollection(context, existingPolicier.Sports, policier.Sports);
            UpdateCollection(context, existingPolicier.DistinctionHonorifiques, policier.DistinctionHonorifiques);
            UpdateCollection(context, existingPolicier.HistAffectations, policier.HistAffectations);
            UpdateCollection(context, existingPolicier.HistFonctions, policier.HistFonctions);
            UpdateCollection(context, existingPolicier.HistGrades, policier.HistGrades);
            UpdateCollection(context, existingPolicier.PersonnePrevenirs, policier.PersonnePrevenirs);
            UpdateCollection(context, existingPolicier.Empreintes, policier.Empreintes);
            UpdateCollection(context, existingPolicier.Fris, policier.Fris);
            
            await context.SaveChangesAsync();
            return policier;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la mise à jour du policier: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeletePolicierAsync(string id)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var policier = await context.Policiers
                .Include(p => p.Conjoints)
                .Include(p => p.Enfants)
                .Include(p => p.Formations)
                .Include(p => p.Langues)
                .Include(p => p.Sports)
                .Include(p => p.DistinctionHonorifiques)
                .Include(p => p.HistAffectations)
                .Include(p => p.PersonnePrevenirs)
                .Include(p => p.Empreintes)
                .FirstOrDefaultAsync(p => p.Id == id);
            
            if (policier == null)
                return false;
            
            context.Policiers.Remove(policier);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la suppression du policier: {ex.Message}");
            return false;
        }
    }

    #endregion

    #region Opérations sur les collections

    public async Task<bool> AddConjointToPolicierAsync(string policierId, Conjoint conjoint)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var policier = await context.Policiers.FindAsync(policierId);
            if (policier == null) return false;
            
            conjoint.IdPolicier = policierId;
            context.Conjoints.Add(conjoint);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'ajout du conjoint: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RemoveConjointFromPolicierAsync(string policierId, string conjointId)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var conjoint = await context.Conjoints
                .FirstOrDefaultAsync(c => c.Id == conjointId && c.IdPolicier == policierId);
            
            if (conjoint == null) return false;
            
            context.Conjoints.Remove(conjoint);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la suppression du conjoint: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateConjointAsync(Conjoint conjoint)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var existingConjoint = await context.Conjoints.FindAsync(conjoint.Id);
            if (existingConjoint == null) return false;
            
            context.Entry(existingConjoint).CurrentValues.SetValues(conjoint);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la mise à jour du conjoint: {ex.Message}");
            return false;
        }
    }

    // Méthodes similaires pour les autres collections...
    // (Pour la concision, je n'ai pas répété toutes les méthodes, mais elles suivent le même pattern)

    public async Task<bool> AddEnfantToPolicierAsync(string policierId, Enfant enfant)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var policier = await context.Policiers.FindAsync(policierId);
            if (policier == null) return false;
            
            enfant.IdPolicier = policierId;
            context.Enfants.Add(enfant);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'ajout de l'enfant: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RemoveEnfantFromPolicierAsync(string policierId, string enfantId)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var enfant = await context.Enfants
                .FirstOrDefaultAsync(e => e.Id == enfantId && e.IdPolicier == policierId);
            
            if (enfant == null) return false;
            
            context.Enfants.Remove(enfant);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la suppression de l'enfant: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateEnfantAsync(Enfant enfant)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var existingEnfant = await context.Enfants.FindAsync(enfant.Id);
            if (existingEnfant == null) return false;
            
            context.Entry(existingEnfant).CurrentValues.SetValues(enfant);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la mise à jour de l'enfant: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> AddFormationToPolicierAsync(string policierId, Formation formation)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var policier = await context.Policiers.FindAsync(policierId);
            if (policier == null) return false;
            
            formation.IdPolicier = policierId;
            context.Formations.Add(formation);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'ajout de la formation: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RemoveFormationFromPolicierAsync(string policierId, string formationId)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var formation = await context.Formations
                .FirstOrDefaultAsync(f => f.Id == formationId && f.IdPolicier == policierId);
            
            if (formation == null) return false;
            
            context.Formations.Remove(formation);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la suppression de la formation: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateFormationAsync(Formation formation)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var existingFormation = await context.Formations.FindAsync(formation.Id);
            if (existingFormation == null) return false;
            
            context.Entry(existingFormation).CurrentValues.SetValues(formation);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la mise à jour de la formation: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> AddLangueToPolicierAsync(string policierId, Langue langue)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var policier = await context.Policiers.FindAsync(policierId);
            if (policier == null) return false;
            
            langue.IdPolicier = policierId;
            context.Langues.Add(langue);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'ajout de la langue: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RemoveLangueFromPolicierAsync(string policierId, string langueId)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var langue = await context.Langues
                .FirstOrDefaultAsync(l => l.Id == langueId && l.IdPolicier == policierId);
            
            if (langue == null) return false;
            
            context.Langues.Remove(langue);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la suppression de la langue: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateLangueAsync(Langue langue)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var existingLangue = await context.Langues.FindAsync(langue.Id);
            if (existingLangue == null) return false;
            
            context.Entry(existingLangue).CurrentValues.SetValues(langue);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la mise à jour de la langue: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> AddSportToPolicierAsync(string policierId, Sport sport)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var policier = await context.Policiers.FindAsync(policierId);
            if (policier == null) return false;
            
            sport.IdPolicier = policierId;
            context.Sports.Add(sport);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'ajout du sport: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RemoveSportFromPolicierAsync(string policierId, string sportId)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var sport = await context.Sports
                .FirstOrDefaultAsync(s => s.Id == sportId && s.IdPolicier == policierId);
            
            if (sport == null) return false;
            
            context.Sports.Remove(sport);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la suppression du sport: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateSportAsync(Sport sport)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var existingSport = await context.Sports.FindAsync(sport.Id);
            if (existingSport == null) return false;
            
            context.Entry(existingSport).CurrentValues.SetValues(sport);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la mise à jour du sport: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> AddDistinctionToPolicierAsync(string policierId, DistinctionHonorifique distinction)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var policier = await context.Policiers.FindAsync(policierId);
            if (policier == null) return false;
            
            distinction.IdPolicier = policierId;
            context.DistinctionHonorifiques.Add(distinction);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'ajout de la distinction: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RemoveDistinctionFromPolicierAsync(string policierId, string distinctionId)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var distinction = await context.DistinctionHonorifiques
                .FirstOrDefaultAsync(d => d.Id == distinctionId && d.IdPolicier == policierId);
            
            if (distinction == null) return false;
            
            context.DistinctionHonorifiques.Remove(distinction);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la suppression de la distinction: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateDistinctionAsync(DistinctionHonorifique distinction)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var existingDistinction = await context.DistinctionHonorifiques.FindAsync(distinction.Id);
            if (existingDistinction == null) return false;
            
            context.Entry(existingDistinction).CurrentValues.SetValues(distinction);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la mise à jour de la distinction: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> AddHistAffectationToPolicierAsync(string policierId, HistAffectation affectation)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var policier = await context.Policiers.FindAsync(policierId);
            if (policier == null) return false;
            
            affectation.IdPolicier = policierId;
            context.HistAffectations.Add(affectation);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'ajout de l'affectation: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RemoveHistAffectationFromPolicierAsync(string policierId, string affectationId)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var affectation = await context.HistAffectations
                .FirstOrDefaultAsync(h => h.Id == affectationId && h.IdPolicier == policierId);
            
            if (affectation == null) return false;
            
            context.HistAffectations.Remove(affectation);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la suppression de l'affectation: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateHistAffectationAsync(HistAffectation affectation)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var existingAffectation = await context.HistAffectations.FindAsync(affectation.Id);
            if (existingAffectation == null) return false;
            
            context.Entry(existingAffectation).CurrentValues.SetValues(affectation);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la mise à jour de l'affectation: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> AddPersonnePrevenirToPolicierAsync(string policierId, PersonnePrevenir personne)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var policier = await context.Policiers.FindAsync(policierId);
            if (policier == null) return false;
            
            personne.IdPolicier = policierId;
            context.PersonnePrevenirs.Add(personne);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'ajout de la personne à prévenir: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RemovePersonnePrevenirFromPolicierAsync(string policierId, string personneId)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var personne = await context.PersonnePrevenirs
                .FirstOrDefaultAsync(p => p.Id == personneId && p.IdPolicier == policierId);
            
            if (personne == null) return false;
            
            context.PersonnePrevenirs.Remove(personne);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la suppression de la personne à prévenir: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdatePersonnePrevenirAsync(PersonnePrevenir personne)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var existingPersonne = await context.PersonnePrevenirs.FindAsync(personne.Id);
            if (existingPersonne == null) return false;
            
            context.Entry(existingPersonne).CurrentValues.SetValues(personne);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la mise à jour de la personne à prévenir: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> AddEmpreinteToPolicierAsync(string policierId, Empreinte empreinte)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var policier = await context.Policiers.FindAsync(policierId);
            if (policier == null) return false;
            
            empreinte.IdPolicier = policierId;
            context.Empreintes.Add(empreinte);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'ajout de l'empreinte: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RemoveEmpreinteFromPolicierAsync(string policierId, string empreinteId)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var empreinte = await context.Empreintes
                .FirstOrDefaultAsync(e => e.Id == empreinteId && e.IdPolicier == policierId);
            
            if (empreinte == null) return false;
            
            context.Empreintes.Remove(empreinte);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la suppression de l'empreinte: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateEmpreinteAsync(Empreinte empreinte)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var existingEmpreinte = await context.Empreintes.FindAsync(empreinte.Id);
            if (existingEmpreinte == null) return false;
            
            context.Entry(existingEmpreinte).CurrentValues.SetValues(empreinte);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la mise à jour de l'empreinte: {ex.Message}");
            return false;
        }
    }

    #endregion

    #region Opérations de validation et vérification

    public async Task<bool> IsMatriculeUniqueAsync(string matricule, string? excludeId = null)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        
        if (string.IsNullOrEmpty(excludeId))
            return !await context.Policiers.AnyAsync(p => p.Matricule == matricule);
        
        return !await context.Policiers.AnyAsync(p => p.Matricule == matricule && p.Id != excludeId);
    }

    public async Task<bool> IsNutpUniqueAsync(string numeroNutp, string? excludeId = null)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        
        if (string.IsNullOrEmpty(excludeId))
            return !await context.Policiers.AnyAsync(p => p.NumeroNutp == numeroNutp);
        
        return !await context.Policiers.AnyAsync(p => p.NumeroNutp == numeroNutp && p.Id != excludeId);
    }

    public async Task<bool> PolicierExistsAsync(string id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Policiers.AnyAsync(p => p.Id == id);
    }

    #endregion

    #region Opérations de sauvegarde

    public async Task<bool> SaveChangesAsync()
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la sauvegarde: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ValidateAndSavePolicierAsync(Policier policier)
    {
        try
        {
            // Valider toutes les étapes
            var validationResult = _validationService.ValidateAllSteps(policier);
            
            if (!validationResult.IsValid)
            {
                Console.WriteLine($"Validation échouée: {validationResult}");
                return false;
            }
            
            // Sauvegarder en base
            Policier? savedPolicier;
            if (string.IsNullOrEmpty(policier.Id))
                savedPolicier = await CreatePolicierAsync(policier);
            else
                savedPolicier = await UpdatePolicierAsync(policier);
                
            return savedPolicier != null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la validation et sauvegarde: {ex.Message}");
            return false;
        }
    }

    #endregion

    #region Méthodes privées

    private void UpdateCollection<T>(BdPolicePncContext context, ICollection<T> existingCollection, ICollection<T> newCollection) where T : class
    {
        if (newCollection == null) return;
        
        // Supprimer les éléments qui ne sont plus dans la nouvelle collection
        var itemsToRemove = existingCollection.Where(item => !newCollection.Contains(item)).ToList();
        foreach (var item in itemsToRemove)
        {
            existingCollection.Remove(item);
            context.Remove(item);
        }
        
        // Ajouter ou mettre à jour les éléments
        foreach (var newItem in newCollection)
        {
            var existingItem = existingCollection.FirstOrDefault(item => 
                context.Entry(item).Property("Id").CurrentValue.ToString() == 
                context.Entry(newItem).Property("Id").CurrentValue.ToString());
            
            if (existingItem == null)
            {
                existingCollection.Add(newItem);
            }
            else
            {
                context.Entry(existingItem).CurrentValues.SetValues(newItem);
            }
        }
    }

    /// <summary>
    /// Crée automatiquement un FRI initial pour un nouveau policier
    /// </summary>
    private async Task CreateInitialFriAsync(BdPolicePncContext context, Policier policier)
    {
        try
        {
            var today = DateTime.Today;
            
            // Créer le FRI initial
            var fri = new Fri
            {
                Id = GenerateShortId(),
                DateRemplissage = today,
                Jour = today.Day,
                Mois = today.ToString("MMMM", new System.Globalization.CultureInfo("fr-FR")),
                Annee = today.Year,
                Numero = await GenerateNextFriNumberAsync(context),
                IdPolicier = policier.Id,
                IdCommissariat = policier.IdCommissariat ?? "",
                IdEnqueteur = "1d1fc19iip", // Enqueteur existant
            };
            
            context.Fris.Add(fri);
            Console.WriteLine($"✅ FRI automatique créé pour le policier {policier.Nom} {policier.Prenom} - Numéro: {fri.Numero}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Erreur lors de la création du FRI automatique: {ex.Message}");
            // Ne pas empêcher la création du policier même si le FRI échoue
        }
    }
    
    /// <summary>
    /// Génère le prochain numéro de FRI automatiquement (format 4 caractères max)
    /// </summary>
    private async Task<string> GenerateNextFriNumberAsync(BdPolicePncContext context)
    {
        try
        {
            var currentYear = DateTime.Now.Year;
            
            // Compter les FRI existants pour l'année en cours
            var count = await context.Fris
                .Where(f => f.Annee == currentYear)
                .CountAsync();
            
            // Générer un numéro séquentiel sur 4 caractères maximum
            // Format: numéro séquentiel (ex: "0001", "0002", etc.)
            var nextNumber = count + 1;
            
            // Si le nombre dépasse 9999, on utilise les 4 derniers chiffres
            if (nextNumber > 9999)
                nextNumber = nextNumber % 10000;
                
            return nextNumber.ToString("D4"); // Format sur 4 chiffres avec zéros devant
        }
        catch
        {
            // En cas d'erreur, retourner un numéro basique sur 4 caractères
            return DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 4);
        }
    }

    #endregion
    
    #region Méthodes utilitaires publiques
    
    /// <summary>
    /// Génère un ID unique de 10 caractères pour toutes les entités
    /// </summary>
    public string GenerateUniqueId()
    {
        return GenerateShortId();
    }
    
    #endregion
    
    #region Méthodes utilitaires privées
    
    /// <summary>
    /// Génère un ID unique de 10 caractères pour les policiers
    /// </summary>
    private string GenerateShortId()
    {
        // Utiliser les 8 premiers caractères du GUID + 2 caractères aléatoires
        var guid = Guid.NewGuid().ToString("N"); // Format sans tirets
        var random = new Random();
        var randomChars = new string(Enumerable.Range(0, 2)
            .Select(_ => (char)random.Next('A', 'Z' + 1))
            .ToArray());
        
        return guid.Substring(0, 8) + randomChars;
    }


    
    #endregion
}

