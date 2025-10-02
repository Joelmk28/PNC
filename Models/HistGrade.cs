using System;
using System.Collections.Generic;

namespace PNC.Models;

public partial class HistGrade
{
    public string Id { get; set; } = null!;

    public string Intitule { get; set; } = null!;

    public string ActeNomination { get; set; } = null!;

    public DateTime DateNomination { get; set; }

    public DateTime DateActe { get; set; }

    public string IdPolicier { get; set; } = null!;

    public virtual Policier IdPolicierNavigation { get; set; } = null!;
}
