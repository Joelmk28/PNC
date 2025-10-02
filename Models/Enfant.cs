using System;
using System.Collections.Generic;

namespace PNC.Models;

public partial class Enfant
{
    public string Id { get; set; } = null!;

    public string Nom { get; set; } = null!;

    public string? PostNom { get; set; }

    public string? Prenom { get; set; }

    public DateTime DateNaissance { get; set; }

    public string PaysNaissance { get; set; } = null!;

    public string VilleNaissance { get; set; } = null!;

    public string IdPolicier { get; set; } = null!;

    public virtual Policier IdPolicierNavigation { get; set; } = null!;
}
