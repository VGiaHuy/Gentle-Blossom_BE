using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Data.Models;

public partial class ChatRoom
{
    public int ChatRoomId { get; set; }

    public string? ChatRoomName { get; set; }

    public bool IsGroup { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? ChatCode { get; set; }

    public virtual ICollection<ChatRoomUser> ChatRoomUsers { get; set; } = new List<ChatRoomUser>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
