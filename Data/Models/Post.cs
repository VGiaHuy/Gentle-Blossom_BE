using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Data.Models;

public partial class Post
{
    public int PostId { get; set; }

    public int PosterId { get; set; }

    public int CategoryId { get; set; }

    public string Content { get; set; } = null!;

    public DateOnly? CreatedDate { get; set; }

    public int NumberOfLike { get; set; }

    public bool? Hidden { get; set; }

    public virtual PostCategory Category { get; set; } = null!;

    public virtual ICollection<CommentPost> CommentPosts { get; set; } = new List<CommentPost>();

    public virtual ICollection<ConnectionMedical> ConnectionMedicals { get; set; } = new List<ConnectionMedical>();

    public virtual ICollection<PostAnalysis> PostAnalyses { get; set; } = new List<PostAnalysis>();

    public virtual ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();

    public virtual ICollection<PostMedium> PostMedia { get; set; } = new List<PostMedium>();

    public virtual UserProfile Poster { get; set; } = null!;
}
