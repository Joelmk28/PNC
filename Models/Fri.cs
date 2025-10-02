using System;
using System.Collections.Generic;

namespace PNC.Models;

public partial class Fri
{
    public string Id { get; set; } = null!;

    public DateTime DateRemplissage { get; set; }

    public int Jour { get; set; }

    public string Mois { get; set; } = null!;

    public int Annee { get; set; }

    public string Numero { get; set; } = null!;

    public string IdEnqueteur { get; set; } = null!;

    public string IdPolicier { get; set; } = null!;

    public string IdCommissariat { get; set; } = null!;

    public virtual Commissariat IdCommissariatNavigation { get; set; } = null!;

    public virtual Enqueteur IdEnqueteurNavigation { get; set; } = null!;

    public virtual Policier IdPolicierNavigation { get; set; } = null!;
}
