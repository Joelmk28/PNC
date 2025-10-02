using System;
using System.Collections.Generic;

namespace PNC.Models;

public partial class Role
{
    public string Id { get; set; } = null!;

    public string Nom { get; set; } = null!;

    public string? Description { get; set; }

    public bool EstActif { get; set; } = true;

    public DateTime DateCreation { get; set; } = DateTime.Now;

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
