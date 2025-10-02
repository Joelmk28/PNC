using System;
using System.Collections.Generic;

namespace PNC.Models;

public partial class HistAffectation
{
    public string Id { get; set; } = null!;

    public string Lieu { get; set; } = null!;

    public string Denomination { get; set; } = null!;

    public string ActeDenomination { get; set; } = null!;

    public DateTime DateActe { get; set; }

    public string IdPolicier { get; set; } = null!;

    public virtual Policier IdPolicierNavigation { get; set; } = null!;
}
