using System;

namespace PNC.Models;

public partial class Audit
{
    public string Id { get; set; } = null!;

    public string IdUtilisateur { get; set; } = null!;

    public string Action { get; set; } = null!;

    public string Table { get; set; } = null!;

    public string? IdEntite { get; set; }

    public string? AnciennesValeurs { get; set; }

    public string? NouvellesValeurs { get; set; }

    public DateTime DateAction { get; set; } = DateTime.Now;

    public string? AdresseIP { get; set; }

    public string? UserAgent { get; set; }

    public virtual Utilisateur IdUtilisateurNavigation { get; set; } = null!;
}
