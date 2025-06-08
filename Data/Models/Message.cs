using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Data.Models;

public partial class Message
{
    public int MessageId { get; set; }

    public int ChatRoomId { get; set; }

    public int SenderId { get; set; }

    public string? Content { get; set; }

    public bool? HasAttachment { get; set; }

    public DateTime? SentAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ChatRoom ChatRoom { get; set; } = null!;

    public virtual ICollection<MessageAttachment> MessageAttachments { get; set; } = new List<MessageAttachment>();

    public virtual UserProfile Sender { get; set; } = null!;
}
