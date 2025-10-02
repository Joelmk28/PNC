using System;
using System.Collections.Generic;

namespace PNC.Models;

public partial class Langue
{
    public string Id { get; set; } = null!;

    public string Libelle { get; set; } = null!;

    public int NiveauEcriture { get; set; }

    public int NiveauLecture { get; set; }

    public string IdPolicier { get; set; } = null!;

    public virtual Policier IdPolicierNavigation { get; set; } = null!;
}
