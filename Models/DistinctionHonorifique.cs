using System;
using System.Collections.Generic;

namespace PNC.Models;

public partial class DistinctionHonorifique
{
    public string Id { get; set; } = null!;

    public DateTime DateDecision { get; set; }

    public string NumeroDecision { get; set; } = null!;

    public string Motif { get; set; } = null!;

    public string IdPolicier { get; set; } = null!;

    public virtual Policier IdPolicierNavigation { get; set; } = null!;
}
