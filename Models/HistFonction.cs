using System;
using System.Collections.Generic;

namespace PNC.Models;

public partial class HistFonction
{
    public string Id { get; set; } = null!;

    public string IntituleFonction { get; set; } = null!;

    public DateTime DatePriseFonction { get; set; }

    public DateTime DateFin { get; set; }

    public string IdPolicier { get; set; } = null!;

    public virtual Policier IdPolicierNavigation { get; set; } = null!;
}
