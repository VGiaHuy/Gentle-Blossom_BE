using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Data.Models;

public partial class PostAnalysis
{
    public long PostAnalysisId { get; set; }

    public int PostId { get; set; }

    public int Score { get; set; }

    public double? SentimentScore { get; set; }

    public double? SentimentMagnitude { get; set; }

    public string? RiskLevel { get; set; }

    public string? AnalysisStatus { get; set; }

    public virtual Post Post { get; set; } = null!;
}
