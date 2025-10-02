using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PNC.Models;

public partial class Policier
{
    public string Id { get; set; } = null!;

    public string NumeroNutp { get; set; } = null!;

    public string Matricule { get; set; } = null!;

    public string Nom { get; set; } = null!;

    public string? PostNom { get; set; }

    public string? Prenom { get; set; }

    public string Sexe { get; set; } = null!;

    public DateTime DateNaissance { get; set; }

    public string Jour { get; set; } = null!;

    public string Mois { get; set; } = null!;

    public int Annee { get; set; }

    public string PaysNaissance { get; set; } = null!;

    public string VilleNaissance { get; set; } = null!;

    public string? VillageNaissance { get; set; }

    public string NatureGrade { get; set; } = null!;

    public string? ActeNominationGrade { get; set; }

    public DateTime? DateNomination { get; set; }

    public DateTime DateEntreePolice { get; set; }

    public string LieuEntreePolice { get; set; } = null!;

    public string FonctionActuelle { get; set; } = null!;

    public DateTime? DatePriseFonction { get; set; }

    public string UniteMere { get; set; } = null!;

    public string UniteAffectation { get; set; } = null!;

    public string NomUnite { get; set; } = null!;

    public string DeptBnDistGpt { get; set; } = null!;

    public string BuCielCiatEscDet { get; set; } = null!;

    public string SecPiSciatSousDet { get; set; } = null!;

    public string ActeNominationFonction { get; set; } = null!;

    public DateTime? DateActe { get; set; }

    public string LieuAffectation { get; set; } = null!;

    public string EtatCivil { get; set; } = null!;

    public string GroupeSanguin { get; set; } = null!;

    public string Rhesus { get; set; } = null!;

    public string ProvinceOrigine { get; set; } = null!;

    public string DistrictOrigine { get; set; } = null!;

    public string TerritoireOrigine { get; set; } = null!;

    public string SecteurOrigine { get; set; } = null!;

    public string VillageOrigine { get; set; } = null!;

    public string NumeroRue { get; set; } = null!;

    public string Rue { get; set; } = null!;

    public string Commune { get; set; } = null!;

    public string Telephone { get; set; } = null!;

    public string? CategoriePermis { get; set; }

    public string? NumeroPermis { get; set; }

    public DateTime? DateDelivrancePermis { get; set; }

    public string Photo { get; set; } = null!;

    public string Signature { get; set; } = null!;



    public string Observation { get; set; } = null!;

    public string? IdCommissariat { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    public virtual Commissariat? IdCommissariatNavigation { get; set; }

    public virtual ICollection<Conjoint> Conjoints { get; set; } = new List<Conjoint>();

    public virtual ICollection<DistinctionHonorifique> DistinctionHonorifiques { get; set; } = new List<DistinctionHonorifique>();

    public virtual ICollection<Empreinte> Empreintes { get; set; } = new List<Empreinte>();

    public virtual ICollection<Enfant> Enfants { get; set; } = new List<Enfant>();

    public virtual ICollection<Formation> Formations { get; set; } = new List<Formation>();

    public virtual ICollection<Fri> Fris { get; set; } = new List<Fri>();

    public virtual ICollection<HistAffectation> HistAffectations { get; set; } = new List<HistAffectation>();

    public virtual ICollection<HistFonction> HistFonctions { get; set; } = new List<HistFonction>();

    public virtual ICollection<HistGrade> HistGrades { get; set; } = new List<HistGrade>();

    public virtual ICollection<Langue> Langues { get; set; } = new List<Langue>();

    public virtual ICollection<PersonnePrevenir> PersonnePrevenirs { get; set; } = new List<PersonnePrevenir>();

    public virtual ICollection<Sport> Sports { get; set; } = new List<Sport>();

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
}



