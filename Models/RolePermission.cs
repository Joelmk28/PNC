using System;

namespace PNC.Models;

public partial class RolePermission
{
    public string IdRole { get; set; } = null!;

    public string IdPermission { get; set; } = null!;

    public DateTime DateAttribution { get; set; } = DateTime.Now;

    public string? AttribuePar { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual Permission Permission { get; set; } = null!;
}
