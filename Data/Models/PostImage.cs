using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Data.Models;

public partial class PostImage
{
    public int ImageId { get; set; }

    public int PostId { get; set; }

    public string Image { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;
}
