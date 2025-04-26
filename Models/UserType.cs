using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Models;

public partial class UserType
{
    public byte UsertypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();
}
