using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Data.Models;

public partial class CommentPost
{
    public int CommentId { get; set; }

    public int PostId { get; set; }

    public int PosterId { get; set; }

    public int? ParentCommentId { get; set; }

    public string Content { get; set; } = null!;

    public DateOnly? CommentDate { get; set; }

    public string? MediaUrl { get; set; }

    public string? MediaType { get; set; }

    public string? FileName { get; set; }

    public virtual ICollection<CommentPost> InverseParentComment { get; set; } = new List<CommentPost>();

    public virtual CommentPost? ParentComment { get; set; }

    public virtual Post Post { get; set; } = null!;

    public virtual UserProfile Poster { get; set; } = null!;
}
