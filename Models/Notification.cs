using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int UserId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? CreateAt { get; set; }

    public bool? IsSeen { get; set; }

    public virtual UserProfile User { get; set; } = null!;
}
