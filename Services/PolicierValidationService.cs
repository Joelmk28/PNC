using PNC.Models;

namespace PNC.Services;

public interface IPolicierValidationService
{
    ValidationResult ValidateStep1(Policier policier);
    ValidationResult ValidateStep2(Policier policier);
    ValidationResult ValidateStep3(Policier policier);
    ValidationResult ValidateStep4(Policier policier);
    ValidationResult ValidateStep5(Policier policier);
    ValidationResult ValidateStep6(Policier policier);
    ValidationResult ValidateStep7(Policier policier);
    ValidationResult ValidateAllSteps(Policier policier);
}

public class PolicierValidationService : IPolicierValidationService
{
    public ValidationResult ValidateStep1(Policier policier)
    {
        var result = new ValidationResult();
        
        // Validation des champs obligatoires
        if (string.IsNullOrEmpty(policier.NumeroNutp))
        {
            result.AddError("NumeroNutp", "Le numéro NUTP est obligatoire");
        }
        
        if (string.IsNullOrEmpty(policier.Nom))
        {
            result.AddError("Nom", "Le nom est obligatoire");
        }
        
        if (string.IsNullOrEmpty(policier.Prenom))
        {
            result.AddError("Prenom", "Le prénom est obligatoire");
        }
        
        if (string.IsNullOrEmpty(policier.Sexe))
        {
            result.AddError("Sexe", "Le sexe est obligatoire");
        }
        
        if (string.IsNullOrEmpty(policier.EtatCivil))
        {
            result.AddError("EtatCivil", "L'état civil est obligatoire");
        }
        
        // Validation des dates
        if (policier.DateNaissance == default)
        {
            result.AddError("DateNaissance", "La date de naissance est obligatoire");
        }
        else
        {
            if (policier.DateNaissance > DateTime.Today)
            {
                result.AddError("DateNaissance", "La date de naissance ne peut pas être dans le futur");
            }
        }
        
        if (policier.DateEntreePolice == default)
        {
            result.AddError("DateEntreePolice", "La date d'entrée à la police est obligatoire");
        }
        else
        {
            if (policier.DateEntreePolice > DateTime.Today)
            {
                result.AddError("DateEntreePolice", "La date d'entrée à la police ne peut pas être dans le futur");
            }
        }
        
        // Validation de la cohérence des dates et de l'âge
        if (policier.DateNaissance != default && policier.DateEntreePolice != default)
        {
            if (policier.DateEntreePolice < policier.DateNaissance)
            {
                result.AddError("DateEntreePolice", "La date d'entrée à la police ne peut pas être avant la date de naissance");
            }
            else
            {
                var ageEntreePolice = CalculateAge(policier.DateNaissance, policier.DateEntreePolice);
                
                if (ageEntreePolice < 18)
                {
                    result.AddError("DateNaissance", $"L'âge minimum pour entrer à la police est de 18 ans. Âge calculé: {ageEntreePolice} ans");
                }
                else if (ageEntreePolice > 70)
                {
                    result.AddError("DateNaissance", $"L'âge maximum pour entrer à la police est de 70 ans. Âge calculé: {ageEntreePolice} ans");
                }
            }
        }
        
        return result;
    }
    
    public ValidationResult ValidateStep2(Policier policier)
    {
        var result = new ValidationResult();
        
        // Validation des champs obligatoires
        if (string.IsNullOrEmpty(policier.Matricule))
        {
            result.AddError("Matricule", "Le matricule est obligatoire");
        }
        
        if (string.IsNullOrEmpty(policier.NatureGrade))
        {
            result.AddError("NatureGrade", "Le grade est obligatoire");
        }
        
        // Validation des dates professionnelles
        if (policier.DateNomination.HasValue)
        {
            if (policier.DateNomination.Value > DateTime.Today)
            {
                result.AddError("DateNomination", "La date de nomination ne peut pas être dans le futur");
            }
            else if (policier.DateNomination.Value < policier.DateEntreePolice)
            {
                result.AddError("DateNomination", "La date de nomination ne peut pas être avant l'entrée à la police");
            }
        }
        
        if (policier.DatePriseFonction.HasValue)
        {
            if (policier.DatePriseFonction.Value > DateTime.Today)
            {
                result.AddError("DatePriseFonction", "La date de prise de fonction ne peut pas être dans le futur");
            }
            else if (policier.DatePriseFonction.Value < policier.DateEntreePolice)
            {
                result.AddError("DatePriseFonction", "La date de prise de fonction ne peut pas être avant l'entrée à la police");
            }
        }
        
        // Validation de la cohérence entre nomination et prise de fonction
        if (policier.DateNomination.HasValue && policier.DatePriseFonction.HasValue)
        {
            if (policier.DatePriseFonction.Value < policier.DateNomination.Value)
            {
                result.AddError("DatePriseFonction", "La date de prise de fonction ne peut pas être avant la date de nomination");
            }
        }
        
        return result;
    }
    
    public ValidationResult ValidateStep3(Policier policier)
    {
        var result = new ValidationResult();
        
        // Validation du téléphone
        if (!string.IsNullOrEmpty(policier.Telephone))
        {
            if (policier.Telephone.Length < 8)
            {
                result.AddError("Telephone", "Le numéro de téléphone doit contenir au moins 8 chiffres");
            }
        }
        
        // Validation des dates optionnelles
        if (policier.DateDelivrancePermis.HasValue)
        {
            if (policier.DateDelivrancePermis.Value > DateTime.Today)
            {
                result.AddError("DateDelivrancePermis", "La date de délivrance du permis ne peut pas être dans le futur");
            }
            else if (policier.DateDelivrancePermis.Value < policier.DateNaissance)
            {
                result.AddError("DateDelivrancePermis", "La date de délivrance du permis ne peut pas être avant la naissance");
            }
            else
            {
                var agePermis = CalculateAge(policier.DateNaissance, policier.DateDelivrancePermis.Value);
                
                if (agePermis < 16)
                {
                    result.AddError("DateDelivrancePermis", $"L'âge minimum pour obtenir un permis est de 16 ans. Âge calculé: {agePermis} ans");
                }
            }
        }
        
        return result;
    }
    
    public ValidationResult ValidateStep4(Policier policier)
    {
        var result = new ValidationResult();
        
        // Validation des formations
        if (policier.Formations?.Any() == true)
        {
            foreach (var formation in policier.Formations)
            {
                if (string.IsNullOrEmpty(formation.Diplome))
                {
                    result.AddError("Formations", "Tous les diplômes doivent être renseignés");
                    break;
                }
                
                if (string.IsNullOrEmpty(formation.Ecole))
                {
                    result.AddError("Formations", "Toutes les écoles doivent être renseignées");
                    break;
                }
                
                if (formation.Annee < 1950 || formation.Annee > DateTime.Today.Year)
                {
                    result.AddError("Formations", $"L'année de formation doit être entre 1950 et {DateTime.Today.Year}");
                    break;
                }
            }
        }
        
        // Validation des langues
        if (policier.Langues?.Any() == true)
        {
            foreach (var langue in policier.Langues)
            {
                if (string.IsNullOrEmpty(langue.Libelle))
                {
                    result.AddError("Langues", "Tous les noms de langues doivent être renseignés");
                    break;
                }
                
                if (langue.NiveauEcriture < 1 || langue.NiveauEcriture > 5)
                {
                    result.AddError("Langues", "Le niveau d'écriture doit être entre 1 et 5");
                    break;
                }
                
                if (langue.NiveauLecture < 1 || langue.NiveauLecture > 5)
                {
                    result.AddError("Langues", "Le niveau de lecture doit être entre 1 et 5");
                    break;
                }
            }
        }
        
        // Validation des sports
        if (policier.Sports?.Any() == true)
        {
            foreach (var sport in policier.Sports)
            {
                if (string.IsNullOrEmpty(sport.Libelle))
                {
                    result.AddError("Sports", "Tous les noms de sports doivent être renseignés");
                    break;
                }
            }
        }
        
        return result;
    }
    
    public ValidationResult ValidateStep5(Policier policier)
    {
        var result = new ValidationResult();
        
        // Si célibataire, cette étape est automatiquement validée
        if (policier.EtatCivil == "Célibataire")
        {
            return result;
        }
        
        // Validation des conjoints
        if (policier.Conjoints?.Any() == true)
        {
            foreach (var conjoint in policier.Conjoints)
            {
                if (string.IsNullOrEmpty(conjoint.Nom))
                {
                    result.AddError("Conjoints", "Tous les noms des conjoints doivent être renseignés");
                    break;
                }
                
                if (string.IsNullOrEmpty(conjoint.Profession))
                {
                    result.AddError("Conjoints", "Toutes les professions des conjoints doivent être renseignées");
                    break;
                }
                
                if (conjoint.DateMariage.HasValue)
                {
                    if (conjoint.DateMariage.Value > DateTime.Today)
                    {
                        result.AddError("Conjoints", "La date de mariage ne peut pas être dans le futur");
                        break;
                    }
                    
                    if (conjoint.DateMariage.Value < policier.DateEntreePolice)
                    {
                        result.AddError("Conjoints", "La date de mariage ne peut pas être avant l'entrée à la police");
                        break;
                    }
                }
            }
        }
        
        // Validation des enfants
        if (policier.Enfants?.Any() == true)
        {
            foreach (var enfant in policier.Enfants)
            {
                if (string.IsNullOrEmpty(enfant.Nom))
                {
                    result.AddError("Enfants", "Tous les noms des enfants doivent être renseignés");
                    break;
                }
                
                if (enfant.DateNaissance > DateTime.Today)
                {
                    result.AddError("Enfants", "La date de naissance d'un enfant ne peut pas être dans le futur");
                    break;
                }
                
                if (enfant.DateNaissance < policier.DateNaissance)
                {
                    result.AddError("Enfants", "Un enfant ne peut pas être né avant le policier");
                    break;
                }
            }
        }
        
        // Vérifier qu'il y a au moins un conjoint ou un enfant pour les non-célibataires
        var hasFamily = (policier.Conjoints?.Any() == true) || (policier.Enfants?.Any() == true);
        if (!hasFamily)
        {
            result.AddError("Famille", "Veuillez ajouter au moins un conjoint ou un enfant");
        }
        
        return result;
    }
    
    public ValidationResult ValidateStep6(Policier policier)
    {
        var result = new ValidationResult();
        
        // Validation des distinctions honorifiques
        if (policier.DistinctionHonorifiques?.Any() == true)
        {
            foreach (var distinction in policier.DistinctionHonorifiques)
            {
                if (string.IsNullOrEmpty(distinction.NumeroDecision))
                {
                    result.AddError("Distinctions", "Tous les numéros de décision doivent être renseignés");
                    break;
                }
                
                if (string.IsNullOrEmpty(distinction.Motif))
                {
                    result.AddError("Distinctions", "Tous les motifs de distinction doivent être renseignés");
                    break;
                }
                
                if (distinction.DateDecision > DateTime.Today)
                {
                    result.AddError("Distinctions", "La date de décision ne peut pas être dans le futur");
                    break;
                }
                
                if (distinction.DateDecision < policier.DateEntreePolice)
                {
                    result.AddError("Distinctions", "Une distinction ne peut pas être obtenue avant l'entrée à la police");
                    break;
                }
            }
        }
        
        // Validation de l'historique des affectations
        if (policier.HistAffectations?.Any() == true)
        {
            foreach (var affectation in policier.HistAffectations)
            {
                if (string.IsNullOrEmpty(affectation.Denomination))
                {
                    result.AddError("HistAffectations", "Toutes les dénominations d'affectation doivent être renseignées");
                    break;
                }
                
                if (string.IsNullOrEmpty(affectation.Lieu))
                {
                    result.AddError("HistAffectations", "Tous les lieux d'affectation doivent être renseignés");
                    break;
                }
                
                if (affectation.DateActe > DateTime.Today)
                {
                    result.AddError("HistAffectations", "La date d'acte d'affectation ne peut pas être dans le futur");
                    break;
                }
                
                if (affectation.DateActe < policier.DateEntreePolice)
                {
                    result.AddError("HistAffectations", "Une affectation ne peut pas avoir lieu avant l'entrée à la police");
                    break;
                }
            }
        }
        
        return result;
    }
    
    public ValidationResult ValidateStep7(Policier policier)
    {
        var result = new ValidationResult();
        
        // Validation des personnes à prévenir
        if (policier.PersonnePrevenirs?.Any() == true)
        {
            foreach (var personne in policier.PersonnePrevenirs)
            {
                if (string.IsNullOrEmpty(personne.Nom))
                {
                    result.AddError("PersonnePrevenir", "Tous les noms des personnes à prévenir doivent être renseignés");
                    break;
                }
                
                if (string.IsNullOrEmpty(personne.Telephone))
                {
                    result.AddError("PersonnePrevenir", "Tous les téléphones des personnes à prévenir doivent être renseignés");
                    break;
                }
                
                if (personne.Telephone.Length < 8)
                {
                    result.AddError("PersonnePrevenir", "Tous les téléphones doivent contenir au moins 8 chiffres");
                    break;
                }
            }
        }
        
        // Validation des empreintes
        if (policier.Empreintes?.Any() == true)
        {
            foreach (var empreinte in policier.Empreintes)
            {
                if (string.IsNullOrEmpty(empreinte.TypeDoigt))
                {
                    result.AddError("Empreintes", "Tous les types de doigts doivent être renseignés");
                    break;
                }
                
                if (string.IsNullOrEmpty(empreinte.Urlepreinte))
                {
                    result.AddError("Empreintes", "Toutes les URLs d'empreintes doivent être renseignées");
                    break;
                }
            }
        }
        
        // Validation des observations
        if (!string.IsNullOrEmpty(policier.Observation) && policier.Observation.Length > 1000)
        {
            result.AddError("Observation", "Les observations ne peuvent pas dépasser 1000 caractères");
        }
        
        return result;
    }
    
    public ValidationResult ValidateAllSteps(Policier policier)
    {
        var result = new ValidationResult();
        
        // Valider toutes les étapes
        var step1Result = ValidateStep1(policier);
        var step2Result = ValidateStep2(policier);
        var step3Result = ValidateStep3(policier);
        var step4Result = ValidateStep4(policier);
        var step5Result = ValidateStep5(policier);
        var step6Result = ValidateStep6(policier);
        var step7Result = ValidateStep7(policier);
        
        // Combiner tous les résultats
        result.Merge(step1Result);
        result.Merge(step2Result);
        result.Merge(step3Result);
        result.Merge(step4Result);
        result.Merge(step5Result);
        result.Merge(step6Result);
        result.Merge(step7Result);
        
        return result;
    }
    
    private int CalculateAge(DateTime dateNaissance, DateTime dateReference)
    {
        var age = dateReference.Year - dateNaissance.Year;
        if (dateReference < dateNaissance.AddYears(age))
        {
            age--;
        }
        return age;
    }
}
