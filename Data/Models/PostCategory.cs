using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Data.Models;

public partial class PostCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
