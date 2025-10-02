using System;
using System.Collections.Generic;

namespace PNC.Models;

public partial class Commissariat
{
    public string Id { get; set; } = null!;

    public string? Nom { get; set; }

    public string? Province { get; set; }

    public string? Territoire { get; set; }

    public string? District { get; set; }

    public virtual ICollection<FonctionBefore1997> FonctionBefore1997s { get; set; } = new List<FonctionBefore1997>();

    public virtual ICollection<Fri> Fris { get; set; } = new List<Fri>();
}
