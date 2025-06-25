using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Data.Models;

public partial class UserProfile
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool Gender { get; set; }

    public byte UserTypeId { get; set; }

    public string? AvatarUrl { get; set; }

    public string? AvatarType { get; set; }

    public string? AvatarFileName { get; set; }

    public virtual Administrator? Administrator { get; set; }

    public virtual ICollection<ChatRoomUser> ChatRoomUsers { get; set; } = new List<ChatRoomUser>();

    public virtual ICollection<CommentPost> CommentPosts { get; set; } = new List<CommentPost>();

    public virtual ICollection<ConnectionMedical> ConnectionMedicals { get; set; } = new List<ConnectionMedical>();

    public virtual Expert? Expert { get; set; }

    public virtual ICollection<HealthJourney> HealthJourneys { get; set; } = new List<HealthJourney>();

    public virtual LoginUser? LoginUser { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual UserType UserType { get; set; } = null!;
}
