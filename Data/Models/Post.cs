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

    public virtual PostCategory Category { get; set; } = null!;

    public virtual ICollection<CommentPost> CommentPosts { get; set; } = new List<CommentPost>();

    public virtual ICollection<PostImage> PostImages { get; set; } = new List<PostImage>();

    public virtual UserProfile Poster { get; set; } = null!;
}
