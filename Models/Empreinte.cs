using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PNC.Models;

public partial class Empreinte
{
    public string Id { get; set; } = null!;

    public string IdPolicier { get; set; } = null!;

    public string TypeDoigt { get; set; } = null!;

    
    public string Urlepreinte { get; set; } = null!; 

    public virtual Policier IdPolicierNavigation { get; set; } = null!;
}
