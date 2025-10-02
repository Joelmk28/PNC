using System;
using System.Collections.Generic;

namespace PNC.Models;

public partial class Enqueteur
{
    public string Id { get; set; } = null!;

    public string CodeIdentification { get; set; } = null!;

    public string Nom { get; set; } = null!;

    public string PostNom { get; set; } = null!;

    public string? Prenom { get; set; }

    public string? Telephone { get; set; }

    public string? Adresse { get; set; }

    public virtual ICollection<Fri> Fris { get; set; } = new List<Fri>();
}
