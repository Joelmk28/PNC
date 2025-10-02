using PNC.Models;

namespace PNC.Services;

public interface IPolicierCollectionService
{
    void AddConjoint(Policier policier);
    void RemoveConjoint(Policier policier, Conjoint conjoint);
    void AddEnfant(Policier policier);
    void RemoveEnfant(Policier policier, Enfant enfant);
    void AddFormation(Policier policier);
    void RemoveFormation(Policier policier, Formation formation);
    void AddLangue(Policier policier);
    void RemoveLangue(Policier policier, Langue langue);
    void AddSport(Policier policier);
    void RemoveSport(Policier policier, Sport sport);
    void AddDistinction(Policier policier);
    void RemoveDistinction(Policier policier, DistinctionHonorifique distinction);
    void AddHistAffectation(Policier policier);
    void RemoveHistAffectation(Policier policier, HistAffectation affectation);
    void AddPersonnePrevenir(Policier policier);
    void RemovePersonnePrevenir(Policier policier, PersonnePrevenir personne);
    void AddEmpreinte(Policier policier);
    void RemoveEmpreinte(Policier policier, Empreinte empreinte);
}

public class PolicierCollectionService : IPolicierCollectionService
{
    public void AddConjoint(Policier policier)
    {
        if (policier.Conjoints == null)
            policier.Conjoints = new List<Conjoint>();
            
                    var conjoint = new Conjoint
            {
                Id = GenerateShortId(),
            Nom = "",
            PostNom = "",
            Prenom = "",
            Nationalite = "",
            Profession = "",
            Matricule = "",
            DateMariage = null,
            IdPolicier = policier.Id
        };
        
        policier.Conjoints.Add(conjoint);
    }
    
    public void RemoveConjoint(Policier policier, Conjoint conjoint)
    {
        policier.Conjoints?.Remove(conjoint);
    }
    
    public void AddEnfant(Policier policier)
    {
        if (policier.Enfants == null)
            policier.Enfants = new List<Enfant>();
            
                    var enfant = new Enfant
            {
                Id = GenerateShortId(),
            Nom = "",
            PostNom = "",
            Prenom = "",
            DateNaissance = DateTime.Today,
            PaysNaissance = "",
            VilleNaissance = "",
            IdPolicier = policier.Id
        };
        
        policier.Enfants.Add(enfant);
    }
    
    public void RemoveEnfant(Policier policier, Enfant enfant)
    {
        policier.Enfants?.Remove(enfant);
    }
    
    public void AddFormation(Policier policier)
    {
        if (policier.Formations == null)
            policier.Formations = new List<Formation>();
            
                    var formation = new Formation
            {
                Id = GenerateShortId(),
            TypeFormation = "",
            Ecole = "",
            Pays = "",
            Ville = "",
            Annee = DateTime.Today.Year,
            Diplome = "",
            NomDiplome = "",
            Duree = "",
            Nature = "",
            IdPolicier = policier.Id
        };
        
        policier.Formations.Add(formation);
    }
    
    public void RemoveFormation(Policier policier, Formation formation)
    {
        policier.Formations?.Remove(formation);
    }
    
    public void AddLangue(Policier policier)
    {
        if (policier.Langues == null)
            policier.Langues = new List<Langue>();
            
                    var langue = new Langue
            {
                Id = GenerateShortId(),
            Libelle = "",
            NiveauEcriture = 1,
            NiveauLecture = 1,
            IdPolicier = policier.Id
        };
        
        policier.Langues.Add(langue);
    }
    
    public void RemoveLangue(Policier policier, Langue langue)
    {
        policier.Langues?.Remove(langue);
    }
    
    public void AddSport(Policier policier)
    {
        if (policier.Sports == null)
            policier.Sports = new List<Sport>();
            
                    var sport = new Sport
            {
                Id = GenerateShortId(),
            Libelle = "",
            IdPolicier = policier.Id
        };
        
        policier.Sports.Add(sport);
    }
    
    public void RemoveSport(Policier policier, Sport sport)
    {
        policier.Sports?.Remove(sport);
    }
    
    public void AddDistinction(Policier policier)
    {
        if (policier.DistinctionHonorifiques == null)
            policier.DistinctionHonorifiques = new List<DistinctionHonorifique>();
            
                    var distinction = new DistinctionHonorifique
            {
                Id = GenerateShortId(),
            DateDecision = DateTime.Today,
            NumeroDecision = "",
            Motif = "",
            IdPolicier = policier.Id
        };
        
        policier.DistinctionHonorifiques.Add(distinction);
    }
    
    public void RemoveDistinction(Policier policier, DistinctionHonorifique distinction)
    {
        policier.DistinctionHonorifiques?.Remove(distinction);
    }
    
    public void AddHistAffectation(Policier policier)
    {
        if (policier.HistAffectations == null)
            policier.HistAffectations = new List<HistAffectation>();
            
                    var affectation = new HistAffectation
            {
                Id = GenerateShortId(),
            Lieu = "",
            Denomination = "",
            ActeDenomination = "",
            DateActe = DateTime.Today,
            IdPolicier = policier.Id
        };
        
        policier.HistAffectations.Add(affectation);
    }
    
    public void RemoveHistAffectation(Policier policier, HistAffectation affectation)
    {
        policier.HistAffectations?.Remove(affectation);
    }
    
    public void AddPersonnePrevenir(Policier policier)
    {
        if (policier.PersonnePrevenirs == null)
            policier.PersonnePrevenirs = new List<PersonnePrevenir>();
            
                    var personne = new PersonnePrevenir
            {
                Id = GenerateShortId(),
            Nom = "",
            PostNom = "",
            Prenom = "",
            NumeroRue = "",
            Rue = "",
            Commune = "",
            Telephone = "",
            IdPolicier = policier.Id
        };
        
        policier.PersonnePrevenirs.Add(personne);
    }
    
    public void RemovePersonnePrevenir(Policier policier, PersonnePrevenir personne)
    {
        policier.PersonnePrevenirs?.Remove(personne);
    }
    
    public void AddEmpreinte(Policier policier)
    {
        if (policier.Empreintes == null)
            policier.Empreintes = new List<Empreinte>();
            
                    var empreinte = new Empreinte
            {
                Id = GenerateShortId(),
            IdPolicier = policier.Id,
            TypeDoigt = "",
            Urlepreinte = ""
        };
        
        policier.Empreintes.Add(empreinte);
    }
    
    public void RemoveEmpreinte(Policier policier, Empreinte empreinte)
    {
        policier.Empreintes?.Remove(empreinte);
    }
    
    /// <summary>
    /// Génère un ID unique de 10 caractères pour les entités liées aux policiers
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
}
