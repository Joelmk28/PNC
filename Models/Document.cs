using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PNC.Models;

public partial class Document
{
    [Key]
    [StringLength(10)]
    public string Id { get; set; } = null!;

    [Required]
    [StringLength(10)]
    public string IdPolicier { get; set; } = null!;

    [Required]
    [StringLength(500)]
    public string UrlDocument { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Label { get; set; } = null!;

    [StringLength(200)]
    public string? Description { get; set; }

    public DateTime DateCreation { get; set; } = DateTime.Now;

    public DateTime? DateModification { get; set; }

    [StringLength(50)]
    public string? TypeDocument { get; set; }

    [StringLength(20)]
    public string? Extension { get; set; }

    public long? TailleFichier { get; set; }

    public bool EstActif { get; set; } = true;

    [StringLength(50)]
    public string? Status { get; set; }

    // Navigation property
    [ForeignKey("IdPolicier")]
    public virtual Policier IdPolicierNavigation { get; set; } = null!;
}
