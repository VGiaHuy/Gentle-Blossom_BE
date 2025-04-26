using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Data.Models;

public partial class LoginUser
{
    public int LoginId { get; set; }

    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual UserProfile User { get; set; } = null!;
}
