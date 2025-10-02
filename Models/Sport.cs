using System;
using System.Collections.Generic;

namespace PNC.Models;

public partial class Sport
{
    public string Id { get; set; } = null!;

    public string Libelle { get; set; } = null!;

    public string? IdPolicier { get; set; }

    public virtual Policier? IdPolicierNavigation { get; set; }
}
