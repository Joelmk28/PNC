using System;
using System.Collections.Generic;

namespace PNC.Models;

public partial class PersonnePrevenir
{
    public string Id { get; set; } = null!;

    public string Nom { get; set; } = null!;

    public string? PostNom { get; set; }

    public string? Prenom { get; set; }

    public string NumeroRue { get; set; } = null!;

    public string Rue { get; set; } = null!;

    public string Commune { get; set; } = null!;

    public string Telephone { get; set; } = null!;

    public string IdPolicier { get; set; } = null!;

    public virtual Policier IdPolicierNavigation { get; set; } = null!;
}
