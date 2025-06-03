using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Data.Models;

public partial class PostLike
{
    public int UserId { get; set; }

    public int PostId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Post Post { get; set; } = null!;

    public virtual UserProfile User { get; set; } = null!;
}
