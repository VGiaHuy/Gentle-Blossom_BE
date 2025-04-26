using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Data.Models;

public partial class MonitoringForm
{
    public int FormId { get; set; }

    public int ExpertId { get; set; }

    public int JourneyId { get; set; }

    public byte Status { get; set; }

    public string? Notes { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public virtual Expert Expert { get; set; } = null!;

    public virtual HealthJourney Journey { get; set; } = null!;
}
