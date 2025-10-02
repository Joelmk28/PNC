using System;
using System.Collections.Generic;

namespace PNC.Models;

public partial class Utilisateur
{
    public string Id { get; set; } = null!;

    public string NomUtilisateur { get; set; } = null!;

    public string MotDePasse { get; set; } = null!;

    public string Nom { get; set; } = null!;

    public string? Prenom { get; set; }

    public string? Email { get; set; }

    public string? Telephone { get; set; }

    public bool EstActif { get; set; } = true;

    public DateTime DateCreation { get; set; } = DateTime.Now;

    public DateTime? DerniereConnexion { get; set; }

    public bool MotDePasseModifie { get; set; } = false;

    public string? IdEnqueteur { get; set; }

    public string IdRole { get; set; } = null!;

    public virtual Enqueteur? IdEnqueteurNavigation { get; set; }

    public virtual Role IdRoleNavigation { get; set; } = null!;
}
