using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Models;

public partial class ChatRoomUser
{
    public int ChatRoomUserId { get; set; }

    public int ChatRoomId { get; set; }

    public int ParticipantId { get; set; }

    public DateTime? JoinedAt { get; set; }

    public virtual ChatRoom ChatRoom { get; set; } = null!;

    public virtual UserProfile Participant { get; set; } = null!;
}
