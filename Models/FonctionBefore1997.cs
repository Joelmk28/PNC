using System;
using System.Collections.Generic;

namespace PNC.Models;

public partial class FonctionBefore1997
{
    public string Id { get; set; } = null!;

    public string Ministere { get; set; } = null!;

    public string Service { get; set; } = null!;

    public string? DernierGrade { get; set; }

    public string? NatureGrade { get; set; }

    public string? ActeNomination { get; set; }

    public string? MatriculeOrigine { get; set; }

    public DateTime DateEntree { get; set; }

    public string Lieu { get; set; } = null!;

    public string IdCommissariat { get; set; } = null!;

    public virtual Commissariat IdCommissariatNavigation { get; set; } = null!;
}
