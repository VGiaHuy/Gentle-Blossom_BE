using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Data.Models;

public partial class RoleAdmin
{
    public byte RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Administrator> Administrators { get; set; } = new List<Administrator>();
}
