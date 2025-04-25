using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Models;

public partial class Administrator
{
    public int AdminId { get; set; }

    public int UserId { get; set; }

    public byte RoleId { get; set; }

    public virtual RoleAdmin Role { get; set; } = null!;

    public virtual UserProfile User { get; set; } = null!;
}
