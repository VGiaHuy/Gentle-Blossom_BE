using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Data.Models;

public partial class MentalHealthKeyword
{
    public int KeywordId { get; set; }

    public string Keyword { get; set; } = null!;

    public string Category { get; set; } = null!;

    public int Weight { get; set; }

    public string SeverityLevel { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
