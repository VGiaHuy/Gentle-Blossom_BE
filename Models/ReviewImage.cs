using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Models;

public partial class ReviewImage
{
    public int ImageId { get; set; }

    public int ReviewId { get; set; }

    public string Image { get; set; } = null!;

    public virtual Review Review { get; set; } = null!;
}
