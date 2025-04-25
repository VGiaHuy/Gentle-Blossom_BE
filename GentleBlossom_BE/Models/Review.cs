using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int ExpertId { get; set; }

    public int UserId { get; set; }

    public int Rating { get; set; }

    public string? Feedback { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Expert Expert { get; set; } = null!;

    public virtual ICollection<ReviewImage> ReviewImages { get; set; } = new List<ReviewImage>();

    public virtual UserProfile User { get; set; } = null!;
}
