using System;
using System.Collections.Generic;

namespace PNC.Models;

public partial class Permission
{
    public string Id { get; set; } = null!;

    public string Nom { get; set; } = null!;

    public string? Description { get; set; }

    public string Module { get; set; } = null!;

    public string Action { get; set; } = null!;

    public bool EstActif { get; set; } = true;

    public DateTime DateCreation { get; set; } = DateTime.Now;

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
