using System;
using System.Collections.Generic;

namespace PNC.Models;

public partial class Conjoint
{
    public string Id { get; set; } = null!;

    public string Nom { get; set; } = null!;

    public string? PostNom { get; set; }

    public string? Prenom { get; set; }

    public string Nationalite { get; set; } = null!;

    public string Profession { get; set; } = null!;

    public string? Matricule { get; set; }

    public DateTime? DateMariage { get; set; }

    public string IdPolicier { get; set; } = null!;

    public virtual Policier IdPolicierNavigation { get; set; } = null!;
}
