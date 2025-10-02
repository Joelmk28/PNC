using System;
using System.Collections.Generic;

namespace PNC.Models;

public partial class Formation
{
    public string Id { get; set; } = null!;

    public string TypeFormation { get; set; } = null!;

    public string Ecole { get; set; } = null!;

    public string Pays { get; set; } = null!;

    public string Ville { get; set; } = null!;

    public int Annee { get; set; }

    public string Diplome { get; set; } = null!;

    public string NomDiplome { get; set; } = null!;

    public string Duree { get; set; } = null!;

    public string Nature { get; set; } = null!;

    public string IdPolicier { get; set; } = null!;

    public virtual Policier IdPolicierNavigation { get; set; } = null!;
}
